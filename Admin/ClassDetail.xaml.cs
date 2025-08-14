using BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StudentScoreManager.Admin
{
    /// <summary>
    /// Interaction logic for ClassDetail.xaml
    /// </summary>
    public partial class ClassDetail : Window
    {
        private readonly ProjectPrn212Context _context;
        private readonly Class _selectedClass;

        private List<BusinessLogic.Models.Student> enrolledStudents;
        private List<BusinessLogic.Models.Student> unenrolledStudents;

        public ClassDetail(Class selectedClass)
        {
            InitializeComponent();
            _context = new ProjectPrn212Context();
            _selectedClass = _context.Classes
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                .FirstOrDefault(c => c.ClassId == selectedClass.ClassId) ?? selectedClass;

            LoadClassInfo();
            LoadStudents();
        }

        private void LoadClassInfo()
        {
            txtClassId.Text = _selectedClass.ClassCode;
            txtClassName.Text = _selectedClass.ClassName;
            txtSubject.Text = _selectedClass.Subject?.SubjectName ?? "N/A";
            txtTeacher.Text = _selectedClass.Teacher?.FullName ?? "N/A";
            txtSlot.Text = _selectedClass.Slot.ToString();
        }

        private void LoadStudents()
        {
            enrolledStudents = _context.Enrollments
                .Include(e => e.Student)
                .Where(e => e.ClassId == _selectedClass.ClassId)
                .Select(e => e.Student)
                .ToList();

            var allStudents = _context.Students.ToList();
            unenrolledStudents = allStudents
                .Where(s => !enrolledStudents.Any(e => e.StudentId == s.StudentId))
                .ToList();

            RefreshGrids();
        }

        private void RefreshGrids()
        {
            dgEnrolledStudents.ItemsSource = null;
            dgEnrolledStudents.ItemsSource = enrolledStudents;

            dgUnenrolledStudents.ItemsSource = null;
            dgUnenrolledStudents.ItemsSource = unenrolledStudents;

            txtCurrentCount.Text = $"{enrolledStudents.Count}/{_selectedClass.Slot}";
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            if (enrolledStudents.Count >= _selectedClass.Slot)
            {
                MessageBox.Show("Không thể thêm sinh viên. Lớp đã đủ slot.");
                return;
            }

            var selected = dgUnenrolledStudents.SelectedItem as BusinessLogic.Models.Student;
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để thêm.");
                return;
            }

            enrolledStudents.Add(selected);
            unenrolledStudents.Remove(selected);
            RefreshGrids();
        }

        private void RemoveStudent_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgEnrolledStudents.SelectedItem as BusinessLogic.Models.Student;
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để xoá.");
                return;
            }

            // Kiểm tra xem học sinh có điểm chưa
            var enrollment = _context.Enrollments
                .FirstOrDefault(e => e.ClassId == _selectedClass.ClassId && e.StudentId == selected.StudentId);

            if (enrollment != null)
            {
                bool hasGrade = _context.Grades.Any(g => g.EnrollmentId == enrollment.EnrollmentId);
                if (hasGrade)
                {
                    MessageBox.Show("Không thể xoá sinh viên đã có điểm.");
                    return;
                }
            }

            enrolledStudents.Remove(selected);
            unenrolledStudents.Add(selected);
            RefreshGrids();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var existingEnrollments = _context.Enrollments
                    .Where(e => e.ClassId == _selectedClass.ClassId)
                    .ToList();

                // Lấy danh sách studentId mới
                var newStudentIds = enrolledStudents.Select(s => s.StudentId).ToList();

                // Xoá các Enrollment không còn tồn tại và chưa có điểm
                var toRemove = existingEnrollments
                    .Where(e => !newStudentIds.Contains((int)e.StudentId))
                    .Where(e => !_context.Grades.Any(g => g.EnrollmentId == e.EnrollmentId))
                    .ToList();

                _context.Enrollments.RemoveRange(toRemove);
                _context.SaveChanges();

                // Thêm các học sinh mới
                var existingStudentIds = existingEnrollments.Select(e => e.StudentId).ToHashSet();
                var toAdd = newStudentIds.Where(id => !existingStudentIds.Contains(id)).ToList();

                foreach (var studentId in toAdd)
                {
                    _context.Enrollments.Add(new Enrollment
                    {
                        ClassId = _selectedClass.ClassId,
                        StudentId = studentId
                    });
                }

                _context.SaveChanges();
                MessageBox.Show("Lưu danh sách sinh viên thành công!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi lưu dữ liệu:\n" + ex.Message);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _context.Dispose();
            base.OnClosed(e);
        }
    }
}


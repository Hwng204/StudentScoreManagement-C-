using BusinessLogic.Models;
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
    /// Interaction logic for AddClass.xaml
    /// </summary>
    public partial class AddClass : Window
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        public AddClass()
        {
            InitializeComponent();
            Window_Loaded();
        }

        private void Window_Loaded()
        {
            // Load Subject
            lstSubjects.ItemsSource = _context.Subjects.ToList();
            lstSubjects.DisplayMemberPath = "SubjectName";
            lstSubjects.SelectedValuePath = "SubjectId";

            // Load Teacher
            lstTeachers.ItemsSource = _context.Teachers
                .Where(t => t.Status == "Active")
                .ToList();
            lstTeachers.DisplayMemberPath = "FullName";
            lstTeachers.SelectedValuePath = "TeacherId";
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                var newClass = new Class
                {
                    ClassCode = txtClassCode.Text,
                    ClassName = txtClassName.Text,
                    Slot = int.Parse(txtSlot.Text),
                    SubjectId = (int?)lstSubjects.SelectedValue,
                    TeacherId = (int?)lstTeachers.SelectedValue
                };

                bool isDuplicate = _context.Classes.Any(c => c.ClassCode == newClass.ClassCode);
                if (isDuplicate)
                {
                    MessageBox.Show("Mã lớp đã tồn tại. Vui lòng nhập mã lớp khác.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(newClass.ClassCode) ||
                    string.IsNullOrWhiteSpace(newClass.ClassName) ||
                    newClass.Slot <= 0 ||
                    newClass.SubjectId == null ||
                    newClass.TeacherId == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin lớp học.");
                    return;
                }

                _context.Classes.Add(newClass);
                _context.SaveChanges();

                MessageBox.Show("Thêm lớp học thành công!");

                ClassManagement classManagement = new ClassManagement();
                classManagement.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnTurnOut_Click(object sender, RoutedEventArgs e)
        {
            ClassManagement classManagement = new ClassManagement();
            classManagement.Show();
            this.Close();
        }

        private void txtSearchSubject_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = txtSearchSubject.Text.ToLower();
            lstSubjects.ItemsSource = _context.Subjects.Where(s => s.SubjectName.ToLower().Contains(keyword)).ToList();
        }

        private void txtSearchTeacher_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = txtSearchTeacher.Text.ToLower();
            lstTeachers.ItemsSource = _context.Teachers.Where(t => t.FullName.ToLower().Contains(keyword)&& t.Status=="Active").ToList();
        }
    }
}

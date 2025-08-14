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
    /// Interaction logic for StudentScoreDetail.xaml
    /// </summary>
    public partial class StudentScoreDetail : Window
    {
        private readonly ProjectPrn212Context _context = new();

        public StudentScoreDetail(int studentId, int classId)
        {
            InitializeComponent();
            LoadScores(studentId, classId);
        }

        private void LoadScores(int studentId, int classId)
        {
            // Lấy lớp để biết subjectID
            var selectedClass = _context.Classes
                .Include(c => c.Subject)
                .FirstOrDefault(c => c.ClassId == classId);

            if (selectedClass == null)
            {
                MessageBox.Show("Không tìm thấy lớp.");
                return;
            }

            int subjectId =(int) selectedClass.SubjectId;

            // Lấy các thành phần điểm của môn học
            var components = _context.AssessmentComponents
                .Where(ac => ac.SubjectId == subjectId)
                .ToList();

            // Tìm enrollment của học sinh trong lớp đó
            var enrollment = _context.Enrollments
                .FirstOrDefault(e => e.StudentId == studentId && e.ClassId == classId);

            var grades = enrollment != null
                ? _context.Grades
                    .Where(g => g.EnrollmentId == enrollment.EnrollmentId)
                    .ToList()
                : new List<Grade>();

            // Mapping dữ liệu cho hiển thị
            var scoreList = components.Select(ac => new
            {
                ComponentName = ac.ComponentName,
                Weight = ac.Weight,
                ScoreDisplay = grades
                    .FirstOrDefault(g => g.ComponentId == ac.ComponentId)?.Score?.ToString("0.##") ?? "—"
            }).ToList();

            dgScores.ItemsSource = scoreList;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosed(System.EventArgs e)
        {
            _context.Dispose();
            base.OnClosed(e);
        }
    }
}


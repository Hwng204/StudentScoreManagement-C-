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

namespace StudentScoreManager.Student
{
    /// <summary>
    /// Interaction logic for StudentScoreWindow.xaml
    /// </summary>
    public partial class StudentScoreWindow : Window
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        private readonly BusinessLogic.Models.Student student;
        private readonly BusinessLogic.Models.Class selectedClass;
        private int enrollmentId;

        public StudentScoreWindow(BusinessLogic.Models.Student student, BusinessLogic.Models.Class selectedClass)
        {
            InitializeComponent();
            this.student = student;
            this.selectedClass = selectedClass;
            LoadScores();
        }

        private void LoadScores()
        {
            enrollmentId = _context.Enrollments
                .Where(e => e.StudentId == student.StudentId && e.ClassId == selectedClass.ClassId)
                .Select(e => e.EnrollmentId)
                .FirstOrDefault();

            var components = _context.AssessmentComponents
                .Where(c => c.SubjectId == selectedClass.SubjectId)
                .ToList();

            var scoreData = components.Select(c => new
            {
                ComponentName = c.ComponentName,
                Weight = c.Weight,
                Score = _context.Grades
                    .Where(g => g.EnrollmentId == enrollmentId && g.ComponentId == c.ComponentId)
                    .Select(g => (double?)g.Score)
                    .FirstOrDefault() ?? 0
            }).ToList();

            scoreDataGrid.ItemsSource = scoreData;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClassOfStudent classOfStudent = new ClassOfStudent(student);
            classOfStudent.Show();
            this.Close();
        }
    }
}

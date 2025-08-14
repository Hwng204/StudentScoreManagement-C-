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
using Microsoft.EntityFrameworkCore;

namespace StudentScoreManager.Admin
{
    /// <summary>
    /// Interaction logic for StudentDetail.xaml
    /// </summary>
    public partial class StudentDetail : Window
    {
        private readonly ProjectPrn212Context _context;
        private readonly BusinessLogic.Models.Student _student;

        public StudentDetail(BusinessLogic.Models.Student student)
        {
            InitializeComponent();
            _context = new ProjectPrn212Context();
            _student = _context.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Class)
                        .ThenInclude(c => c.Subject)
                .FirstOrDefault(s => s.StudentId == student.StudentId);

            LoadStudentInfo();
            LoadEnrolledClasses();
        }

        private void LoadStudentInfo()
        {
            txtStudentCode.Text = _student.StudentCode;
            txtFullName.Text = _student.FullName;
            txtEmail.Text = _student.Email;
            txtStatus.Text = _student.Status;
        }

        private void LoadEnrolledClasses()
        {
            var classes = _student.Enrollments
                .Select(e => e.Class)
                .ToList();

            dgClasses.ItemsSource = classes;
        }

        private void ViewScore_Click(object sender, RoutedEventArgs e)
        {
            var selectedClass = dgClasses.SelectedItem as BusinessLogic.Models.Class;
            if (selectedClass == null)
            {
                MessageBox.Show("Vui lòng chọn một lớp.");
                return;
            }

            var scoreWindow = new StudentScoreDetail(_student.StudentId, selectedClass.ClassId);
            scoreWindow.ShowDialog();
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

using BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
using StudentScoreManager.Teacher;
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
    /// Interaction logic for ClassOfStudent.xaml
    /// </summary>
    public partial class ClassOfStudent : Window
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        private readonly BusinessLogic.Models.Student student;

        public ClassOfStudent(BusinessLogic.Models.Student Student)
        {
            InitializeComponent();
            this.student = Student;
            LoadClassesOfStudent();
        }

        private void LoadClassesOfStudent()
        {
            var classList = _context.Enrollments
                .Where(e => e.StudentId == student.StudentId)
                .Include(e => e.Class)                      // Include Class
                    .ThenInclude(c => c.Subject)            // Include Subject của Class
                .Select(e => e.Class)                       // Sau đó mới Select class
                .ToList();

            classDataGrid.ItemsSource = classList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string keyword = searchClassField.Text;
            var classList = _context.Enrollments
                .Where(e => e.StudentId == student.StudentId)
                .Include(e => e.Class)                      // Include Class
                
                    .ThenInclude(c => c.Subject)            // Include Subject của Class
                .Select(e => e.Class)                       // Sau đó mới Select class
                .Where(e =>e.ClassCode.ToLower().Contains(keyword.ToLower()))
                .ToList();

            classDataGrid.ItemsSource = classList;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedClass = classDataGrid.SelectedItem as BusinessLogic.Models.Class;
            if (selectedClass == null)
            {
                MessageBox.Show("Please select a class first.");
                return;
            }

            StudentScoreWindow scoreWindow = new StudentScoreWindow(student, selectedClass);
            scoreWindow.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ProfileStudent profileStudent = new ProfileStudent(student);
            profileStudent.Show();
            this.Close();
        }
    }
}

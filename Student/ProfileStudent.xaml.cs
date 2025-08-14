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
    /// Interaction logic for ProfileStudent.xaml
    /// </summary>
    public partial class ProfileStudent : Window
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        private BusinessLogic.Models.Student _getStudent;
        public ProfileStudent(BusinessLogic.Models.Student student)
        {
            InitializeComponent();
            studentCodeField.Text = student.StudentCode;
            studentNameField.Text = student.FullName;
            emailField.Text = student.Email;
            passwordField.Text = student.Password;
            _getStudent = student;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var _student = _getStudent;
            _student.StudentCode = studentCodeField.Text;
            _student.FullName = studentNameField.Text;
            _student.Email = emailField.Text;
            _student.Password = passwordField.Text;
            _context.Update(_student);
            _context.SaveChanges();
            MessageBox.Show("Save successful");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void Classes_Click(object sender, RoutedEventArgs e)
        {
            ClassOfStudent classOfStudent = new ClassOfStudent(_getStudent);
            classOfStudent.Show();
            this.Close();
        }
    }
}

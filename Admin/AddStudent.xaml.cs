using BusinessLogic.Models;
using StudentScoreManager.Student;
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
using BusinessLogic.Models;
using Microsoft.IdentityModel.Tokens;

namespace StudentScoreManager.Admin
{
    /// <summary>
    /// Interaction logic for AddStudent.xaml
    /// </summary>
    public partial class AddStudent : Window
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        public AddStudent()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StudentManagement studentManagement = new StudentManagement();
            studentManagement.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string studentCode_str = studentCodeField.Text;
            string studentName_str = fullNameField.Text;
            string email_str = emailField.Text;
            string password_str = passwordField.Text;
            string status = null;
            var listStudentEmailExisted = _context.Students.Select(x => x.Email).ToList();
            var listTeacherEmailExisted = _context.Teachers.Select(x => x.Email).ToList();
            foreach (String em in listStudentEmailExisted)
            {
                if (e.Equals(email_str))
                {
                    MessageBox.Show("Email existed.");
                }
            }

            foreach (String em in listTeacherEmailExisted)
            {
                if (e.Equals(email_str))
                {
                    MessageBox.Show("Email existed.");
                }
            }


            if (studentCode_str.IsNullOrEmpty() || studentName_str.IsNullOrEmpty() || email_str.IsNullOrEmpty() || password_str.IsNullOrEmpty())
            {
                MessageBox.Show("Please input into each field!");
                return;
            }
            if (statusActive.IsChecked == true)
            {
                status = "Active";
            }
            else if (statusDeactive.IsChecked == true)
            {
                status = "Deactive";
            }
            else
            {
                MessageBox.Show("Please choose stautus.");
                return;
            }
            bool studentCodeExisted = false;
            
            var listStudentCode = _context.Students.Select(x=>x.StudentCode).ToList();
            foreach (var student in listStudentCode)
            {
                if (student.ToLower().Equals(studentCode_str.ToLower()))
                {
                    studentCodeExisted = true;
                }
            }
            if (studentCodeExisted)
            {
                MessageBox.Show("Student code is existed!");
                return;
            }
            else
            {
                BusinessLogic.Models.Student student = new BusinessLogic.Models.Student();
                student.StudentCode = studentCode_str;
                student.FullName = studentName_str;
                student.Email = email_str;
                student.Password = password_str;
                student.Status = status;
                _context.Students.Add(student);
                _context.SaveChanges();
                MessageBox.Show("Add successful.");
                StudentManagement studentManagement = new StudentManagement();
                studentManagement.Show();
                this.Close();
            }
        }
    }
}

using BusinessLogic.Models;
using Microsoft.IdentityModel.Tokens;
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

namespace StudentScoreManager.Admin
{
    /// <summary>
    /// Interaction logic for UpdateStudent.xaml
    /// </summary>
    public partial class UpdateStudent : Window
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        public UpdateStudent(BusinessLogic.Models.Student student)
        {
            InitializeComponent();
            studentCodeField.Text = student.StudentCode;
            fullNameField.Text = student.FullName;
            emailField.Text = student.Email;
            passwordField.Text = student.Password;
            if(student.Status.Equals("Active"))
            {
                statusActive.IsChecked = true;
            }else if(student.Status.Equals("Deactive"))
            {
                statusDeactive.IsChecked = true;
            }
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
            string fullName_str = fullNameField.Text;
            string email_str = emailField.Text;
            string password_str = passwordField.Text;
            string status_str = null;
            if (statusActive.IsChecked == true)
            {
                status_str = "Active";
            }
            else if (statusDeactive.IsChecked == true)
            {
                status_str = "Deactive";
            }
            else
            {
                MessageBox.Show("Please choose status");
            }
            if (fullName_str.IsNullOrEmpty() || email_str.IsNullOrEmpty() || password_str.IsNullOrEmpty())
            {
                MessageBox.Show("Each field not be empty");
                return;
            }
            else
            {
                BusinessLogic.Models.Student student = _context.Students.FirstOrDefault(x => x.StudentCode.ToLower().Equals(studentCode_str.ToLower()));
                student.FullName = fullName_str;
                student.Email = email_str;
                student.Password = password_str;
                student.Status = status_str;
                _context.Students.Update(student);
                _context.SaveChanges();
                MessageBox.Show("Update successful");
                StudentManagement studentManagement = new StudentManagement();
                studentManagement.Show();
                this.Close();
            }
        }
    }
}

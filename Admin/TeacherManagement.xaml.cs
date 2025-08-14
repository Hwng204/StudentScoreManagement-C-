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
    /// Interaction logic for TeacherManagement.xaml
    /// </summary>
    public partial class TeacherManagement : Window
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        public TeacherManagement()
        {
            InitializeComponent();
            loadList();
        }

        private void loadList()
        {
            var listTeacher = _context.Teachers.ToList();
            list.ItemsSource = listTeacher;
        }

        private void classScr_Click(object sender, RoutedEventArgs e)
        {
            ClassManagement classManagement = new ClassManagement();
            classManagement.Show();
            this.Close();
        }

        private void studentScr_Click(object sender, RoutedEventArgs e)
        {
            StudentManagement studentManagement = new StudentManagement();
            studentManagement.Show();
            this.Close();
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void subjectScr_Click(object sender, RoutedEventArgs e)
        {
            SubjectManagement subjectManagement = new SubjectManagement();
            subjectManagement.Show();
            this.Close();
        }

        private void searchTeacher_Click(object sender, RoutedEventArgs e)
        {
            string keyWordSearchTeacherByCode = keywordSearchTeacher.Text;
            if(keywordSearchTeacher == null)
            {
                MessageBox.Show("Emter keyword to search teacher");
                return;
            }
            else
            {
                var listTeacher = _context.Teachers.Where(x=>x.TeacherCode.Contains(keyWordSearchTeacherByCode)).ToList();
                list.ItemsSource = listTeacher;
                
            }
        }

        private void reload_Click(object sender, RoutedEventArgs e)
        {
            teacherIdField.Text = null;
            teacherCodeField.Text = null;
            fullNameFiled.Text = null;
            emailField.Text = null;
            active.IsChecked = false;
            deactive.IsChecked = false;
            loadList();
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var teacherSelected = list.SelectedValue as BusinessLogic.Models.Teacher;
            if (teacherSelected != null)
            {
                teacherIdField.Text = teacherSelected.TeacherId.ToString();
                teacherCodeField.Text = teacherSelected.TeacherCode;
                fullNameFiled.Text = teacherSelected.FullName;
                emailField.Text = teacherSelected.Email;
                if(teacherSelected.Status == "Active")
                {
                    active.IsChecked = true;
                }else if(teacherSelected.Status == "Deactive")
                {
                    deactive.IsChecked = true;
                }

            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            string teacherCode_string = teacherCodeField.Text;
            string fullName_string = fullNameFiled.Text;
            string email_string = emailField.Text;
            string password_string = passwordField.Text;
            string status_string;
            bool codeExisted = false;
            var listTeacherCode = _context.Teachers.Select(x => x.TeacherCode).ToList();

            var listStudentEmailExisted = _context.Students.Select(x => x.Email).ToList();
            var listTeacherEmailExisted = _context.Teachers.Select(x => x.Email).ToList();
            foreach (String em in listStudentEmailExisted)
            {
                if (e.Equals(email_string))
                {
                    MessageBox.Show("Email existed.");
                }
            }

            foreach (String em in listTeacherEmailExisted)
            {
                if (e.Equals(email_string))
                {
                    MessageBox.Show("Email existed.");
                }
            }

            foreach (String tc in listTeacherCode)
            {
                if (tc.ToLower().Equals(teacherCode_string.ToLower()))
                {
                    codeExisted = true;
                }
            }
            if (active.IsChecked == true && deactive.IsChecked == false)
            {
                status_string = "Active";
            }
            else if (deactive.IsChecked == true && active.IsChecked == false)
            {
                status_string = "Deactive";
            }
            else
            {
                MessageBox.Show("Please choose status of teacher.");
                return;
            }
            if (!codeExisted)
            {
                BusinessLogic.Models.Teacher teacher = new BusinessLogic.Models.Teacher();
                teacher.TeacherCode = teacherCode_string;
                teacher.FullName = fullName_string;
                teacher.Email = email_string;
                teacher.Status = status_string;
                teacher.Password = password_string;
                _context.Teachers.Add(teacher);
                _context.SaveChanges();
                MessageBox.Show("Add successful.");
                loadList();
            }
            else
            {
                MessageBox.Show("Teacher code existed");
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            string teacherId_string = teacherIdField.Text;
            string teacherName_string = fullNameFiled.Text;
            string teacherEmail_string = emailField.Text;
            string statusOfTeacher = null;
            if (active.IsChecked == true && deactive.IsChecked == false)
            {
                statusOfTeacher = "Active";
            } else if(deactive.IsChecked == true && active.IsChecked == false) {
                statusOfTeacher = "Deactive";
            }
            else
            {
                MessageBox.Show("Please choose status of teacher");
            }

            int teacherId_int = Int32.Parse(teacherId_string);
            var teacherObj = _context.Teachers.FirstOrDefault(x=>x.TeacherId == teacherId_int);
            if (teacherObj != null)
            {
                teacherObj.Email = teacherEmail_string;
                teacherObj.Status = statusOfTeacher;
                teacherObj.FullName = teacherName_string;
                _context.Update(teacherObj);
                _context.SaveChanges();
                loadList();
            }
            else
            {
                MessageBox.Show("Please choose a teacher in list");
                return;
            }
            
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            string teacherId_string = teacherIdField.Text;
            if(!teacherId_string.IsNullOrEmpty())
            {
                int teacherId_int = Int32.Parse(teacherId_string);
                var teacherObj = _context.Teachers.FirstOrDefault(x => x.TeacherId==teacherId_int);
                if (teacherObj != null)
                {
                    _context.Remove(teacherObj);
                    _context.SaveChanges();
                    loadList();
                }
            }
            else
            {
                MessageBox.Show("Please choose a teacher in list to delete");
            }
        }
    }
}

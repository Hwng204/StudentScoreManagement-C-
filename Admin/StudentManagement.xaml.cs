using BusinessLogic.Models;
using StudentScoreManager.Admin;
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
    /// Interaction logic for StudentManagement.xaml
    /// </summary>
    public partial class StudentManagement : Window
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        public StudentManagement()
        {
            InitializeComponent();
            loadStudent();
        }

        private void loadStudent()
        {
            var listStudent = _context.Students.ToList();
            list.ItemsSource = listStudent;
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void teacherScr_Click(object sender, RoutedEventArgs e)
        {
            TeacherManagement teacherManagement = new TeacherManagement();
            teacherManagement.Show();
            this.Close();
        }

        private void subjectScr_Click(object sender, RoutedEventArgs e)
        {
            SubjectManagement subjectManagement = new SubjectManagement();
            subjectManagement.Show();
            this.Close();
        }

        private void classScr_Click(object sender, RoutedEventArgs e)
        {
            ClassManagement classManagement = new ClassManagement();
            classManagement.Show();
            this.Close();
        }

        private void searchbtn_Click(object sender, RoutedEventArgs e)
        {
            string keywordToSearch = keyword.Text;
            var listSearchStudent = _context.Students.Where(x=>x.StudentCode.ToLower().Contains(keywordToSearch.ToLower())).ToList();
            list.ItemsSource = listSearchStudent;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            AddStudent addStudent = new AddStudent();
            addStudent.Show();
            this.Close();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            var student = list.SelectedItem as BusinessLogic.Models.Student;
            if (student == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc muốn xóa sinh viên \"{student.FullName}\"?",
                                         "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _context.Remove(student); 
                _context.SaveChanges();
                MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                loadStudent();
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            var student = list.SelectedValue as BusinessLogic.Models.Student;
            if (student != null)
            {
                UpdateStudent updateStudent = new UpdateStudent(student);
                updateStudent.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please choose a student in list to update");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedStudent = list.SelectedItem as BusinessLogic.Models.Student;
            if (selectedStudent == null)
            {
                MessageBox.Show("Vui lòng chọn một sinh viên.");
                return;
            }

            var detailWindow = new StudentDetail(selectedStudent);
            detailWindow.ShowDialog();
        }
    }
}

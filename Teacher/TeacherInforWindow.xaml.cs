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

namespace StudentScoreManager.Teacher
{
    /// <summary>
    /// Interaction logic for TeacherInforWindow.xaml
    /// </summary>
    public partial class TeacherInforWindow : Window
    {
        private BusinessLogic.Models.Teacher getTeacher;
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        public TeacherInforWindow(BusinessLogic.Models.Teacher teacher)
        {
            InitializeComponent();
            getTeacher = teacher;
            teacherCodeField.Text = teacher.TeacherCode;
            teacherNameField.Text = teacher.FullName;
            emailField.Text = teacher.Email;
            passwordField.Text = teacher.Password;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            getTeacher.FullName = teacherNameField.Text;
            getTeacher.Password = passwordField.Text;
            _context.Update(getTeacher);
            _context.SaveChanges();
            MessageBox.Show("Update successful");

            // Reload Teacher from DB
            var updatedTeacher = _context.Teachers.FirstOrDefault(t => t.TeacherId == getTeacher.TeacherId);

            // Mở cửa sổ mới và đóng cửa sổ hiện tại
            var newWindow = new TeacherInforWindow(updatedTeacher);
            newWindow.Show();
            this.Close();
        }

        private void Classes_Click(object sender, RoutedEventArgs e)
        {
            ClassView classView = new ClassView(getTeacher);
            classView.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}

using BusinessLogic.Models;
using StudentScoreManager.Admin;
using StudentScoreManager.Student;
using StudentScoreManager.Teacher;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentScoreManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool loginSuccess = false;
                string email = emailLogin.Text;
                string password = passwordLogin.Password;
                if (email == "a" || password == "a")
                {
                    MessageBox.Show("Đăng nhập thành công.");
                    new ClassManagement().Show();
                    this.Close();
                    return;
                }
                var teacher = _context.Teachers.FirstOrDefault(t => t.Email == email && t.Password == password);
                if (teacher != null && teacher.Status=="Active")
                {
                    MessageBox.Show("Đăng nhập thành công.");
                    TeacherInforWindow teacherInforWindow = new TeacherInforWindow(teacher);
                    teacherInforWindow.Show();
                    this.Close();
                    return;
                }else if(teacher!=null && teacher.Status == "Deactive")
                {
                    MessageBox.Show("Tài khoản bị chặn");
                    return;
                }
                    var student = _context.Students.FirstOrDefault(s => s.Email == email && s.Password == password);
                if (student != null && student.Status == "Active")
                {
                    MessageBox.Show("Đăng nhập thành công.");
                    ProfileStudent profileStudent = new ProfileStudent(student);
                    profileStudent.Show();
                    this.Close();
                    return;
                }
                else if (student != null && student.Status == "Deactive")
                {
                    MessageBox.Show("Tài khoản bị chặn");
                    return;
                }
                if (!loginSuccess)
                {
                    MessageBox.Show("Email và mật khẩu sai.");
                }
            }
            catch(Exception ex)
            { 
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Lỗi đăng nhập server");
            }
        }
    }
}
using BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for ClassManagement.xaml
    /// </summary>
    public partial class ClassManagement : Window
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        public ClassManagement()
        {
            InitializeComponent();
            loadList();
        }

        private void loadList()
        {
            List<Class> listClass = _context.Classes
                .Include(x => x.Subject)
                .Include(x => x.Teacher)
                .ToList();
            list.ItemsSource = listClass;
        }

        private void studentScr_Click(object sender, RoutedEventArgs e)
        {
            StudentManagement studentManagement = new StudentManagement();
            studentManagement.Show();
            this.Close();
        }

        private void teacherScr_Click(object sender, RoutedEventArgs e)
        {
            TeacherManagement teacherManagement = new TeacherManagement();
            teacherManagement.Show();
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

        private void add_Click(object sender, RoutedEventArgs e)
        {
            AddClass addClass = new AddClass();
            addClass.Show();
            this.Close();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            var selectedClass = list.SelectedItem as Class;
            if (selectedClass == null)
            {
                MessageBox.Show("Vui lòng chọn một lớp để cập nhật.");
                return;
            }

            var win = new UpdateClass(selectedClass);
            win.ShowDialog();
            loadList();  
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedClass = list.SelectedItem as Class;

            if (selectedClass == null)
            {
                MessageBox.Show("Vui lòng chọn một lớp để xoá.");
                return;
            }

            // Kiểm tra lớp có đang được sử dụng trong bảng Enrollment không
            var hasEnrollments = _context.Enrollments.Any(e => e.ClassId == selectedClass.ClassId);

            if (hasEnrollments)
            {
                MessageBox.Show("Không thể xoá lớp này vì đã có sinh viên đăng ký.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Hỏi xác nhận người dùng
            var confirmResult = MessageBox.Show(
                $"Bạn có chắc muốn xoá lớp \"{selectedClass.ClassName}\" không?",
                "Xác nhận xoá",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (confirmResult == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Classes.Remove(selectedClass);
                    _context.SaveChanges();

                    MessageBox.Show("Xoá lớp học thành công!");
                    loadList(); // Refresh danh sách
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xoá lớp: " + ex.Message);
                }
            }
        }

        private void Enrollment_Click(object sender, RoutedEventArgs e)
        {
            var selectedClass = list.SelectedItem as Class;
            if (selectedClass != null)
            {
                var detailWindow = new ClassDetail(selectedClass);
                detailWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một lớp học.");
            }
        }
    }
}

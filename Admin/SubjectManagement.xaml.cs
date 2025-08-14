using BusinessLogic.Models;
using Microsoft.Data.SqlClient;
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
    /// Interaction logic for SubjectManagement.xaml
    /// </summary>
    public partial class SubjectManagement : Window
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        public SubjectManagement()
        {
            InitializeComponent();
            loadList();
        }

        private void loadList()
        {
            var listSubject = _context.Subjects.ToList();
            list.ItemsSource = listSubject;
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

        private void searchBtb_Click(object sender, RoutedEventArgs e)
        {
            string codeToSearch = searchSubject.Text;
            
            var listSubject = _context.Subjects.Where(x=>x.SubjectCode.Contains(codeToSearch)).ToList();
            list.ItemsSource = listSubject;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            AddSubject addSubjectWindow = new AddSubject();
            addSubjectWindow.ShowDialog();
            loadList(); 
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            var selectedSubject = list.SelectedItem as Subject;
            if (selectedSubject != null)
            {
                UpdateSubject updateWindow = new UpdateSubject(selectedSubject);
                updateWindow.ShowDialog();
                loadList();
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            var subject = list.SelectedValue as Subject;
            if (subject == null)
            {
                MessageBox.Show("Please choose one subject in the list to delete!");
                return;
            }

            // Xác nhận xoá
            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete subject \"{subject.SubjectName}\"?\nThis will also delete all related assessment components.",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Xoá tất cả AssessmentComponent liên quan
                    var components = _context.AssessmentComponents
                                             .Where(ac => ac.SubjectId == subject.SubjectId)
                                             .ToList();
                    _context.AssessmentComponents.RemoveRange(components);

                    // Xoá subject
                    _context.Subjects.Remove(subject);

                    _context.SaveChanges();
                    loadList();
                    MessageBox.Show("Subject and its related assessment components deleted successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occurred: " + ex.Message);
                }
            }
        }
    }
}

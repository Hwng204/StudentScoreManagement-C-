using BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for ClassView.xaml
    /// </summary>

    public partial class ClassView : Window
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        private BusinessLogic.Models.Teacher getTeacher;
        public ClassView(BusinessLogic.Models.Teacher getTeacher)
        {
            InitializeComponent();
            this.getTeacher = getTeacher;
            LoadClassData();
        }

        private void LoadClassData()
        {
            try
            {
                var classes = _context.Classes
                     .Include(c => c.Subject)  // Lấy thêm thông tin môn học
                     .Where(c => c.TeacherId == getTeacher.TeacherId)
                     .ToList();

                classDataGrid.ItemsSource = classes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading class list: " + ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string keyword = searchClassField.Text;
            try
            {
                var classes = _context.Classes
                     .Include(c => c.Subject)  // Lấy thêm thông tin môn học
                     .Where(c => c.TeacherId == getTeacher.TeacherId && c.ClassCode.Contains(keyword))
                     .ToList();

                classDataGrid.ItemsSource = classes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading class list: " + ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedClass = classDataGrid.SelectedItem as Class;
            if (selectedClass != null)
            {
                var StudenstInClass = new StudenstInClass(selectedClass, getTeacher);
                StudenstInClass.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a class to view.");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TeacherInforWindow teacherInforWindow = new TeacherInforWindow(getTeacher);
            teacherInforWindow.Show();
            this.Close();
        }
    }
}

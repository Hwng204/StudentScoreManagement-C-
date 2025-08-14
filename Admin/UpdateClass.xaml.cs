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

namespace StudentScoreManager.Admin
{
    /// <summary>
    /// Interaction logic for UpdateClass.xaml
    /// </summary>
    public partial class UpdateClass : Window
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        private Class _class;


        private List<Subject> _allSubjects;
        private List<BusinessLogic.Models.Teacher> _allTeachers;

        public UpdateClass(Class cls)
        {
            InitializeComponent();
            _class = _context.Classes
                              .Where(c => c.ClassId == cls.ClassId)
                              .FirstOrDefault();
            LoadData();
            BindData();
        }

        private void LoadData()
        {
            _allSubjects = _context.Subjects.ToList();
            _allTeachers = _context.Teachers.ToList();

            lstSubjects.ItemsSource = _allSubjects;
            lstTeachers.ItemsSource = _allTeachers;
        }

        private void BindData()
        {
            // Thông tin lớp
            txtClassCode.Text = _class.ClassCode;
            txtClassName.Text = _class.ClassName;
            txtSlot.Text = _class.Slot.ToString();

            // Chọn sẵn Subject & Teacher
            if (_class.SubjectId.HasValue)
                lstSubjects.SelectedValue = _class.SubjectId.Value;
            if (_class.TeacherId.HasValue)
                lstTeachers.SelectedValue = _class.TeacherId.Value;
        }

        private void txtSearchSubject_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var kw = txtSearchSubject.Text.ToLower();
            lstSubjects.ItemsSource = _allSubjects
                .Where(s => s.SubjectName.ToLower().Contains(kw))
                .ToList();
        }

        private void txtSearchTeacher_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var kw = txtSearchTeacher.Text.ToLower();
            lstTeachers.ItemsSource = _allTeachers
                .Where(t => t.FullName.ToLower().Contains(kw))
                .ToList();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtClassName.Text)
             || !int.TryParse(txtSlot.Text, out int slot)
             || lstSubjects.SelectedValue == null
             || lstTeachers.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin và chọn Subject/Teacher.");
                return;
            }

            // Cập nhật
            _class.ClassName = txtClassName.Text.Trim();
            _class.Slot = slot;
            _class.SubjectId = (int)lstSubjects.SelectedValue;
            _class.TeacherId = (int)lstTeachers.SelectedValue;

            try
            {
                _context.Classes.Update(_class);
                _context.SaveChanges();
                MessageBox.Show("Cập nhật lớp học thành công.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}


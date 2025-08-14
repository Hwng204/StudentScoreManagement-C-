using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StudentScoreManager.Admin
{
    public partial class UpdateSubject : Window
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        private ObservableCollection<AssessmentComponent> components;
        private Subject subjectToEdit;

        public UpdateSubject(Subject subject)
        {
            InitializeComponent();
            subjectToEdit = subject;

            subjectCodeField.Text = subject.SubjectCode;
            subjectNameField.Text = subject.SubjectName;
            subjectCodeField.IsEnabled = false;

            // Load các component hiện tại (không tracking để tránh lỗi sau này)
            var list = _context.AssessmentComponents
                .Where(c => c.SubjectId == subject.SubjectId)
                .Select(c => new AssessmentComponent
                {
                    ComponentId = c.ComponentId,
                    SubjectId = c.SubjectId,
                    ComponentName = c.ComponentName,
                    Weight = c.Weight
                })
                .ToList();

            components = new ObservableCollection<AssessmentComponent>(list);
            componentList.ItemsSource = components;
        }

        private void AddComponent_Click(object sender, RoutedEventArgs e)
        {
            components.Add(new AssessmentComponent { ComponentName = "", Weight = 0 });
        }

        private void DeleteComponent_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is AssessmentComponent comp)
            {
                // Nếu component này đã có trong DB thì kiểm tra xem có Grade liên quan không
                if (comp.ComponentId != 0)
                {
                    bool hasGrades = _context.Grades.Any(g => g.ComponentId == comp.ComponentId);
                    if (hasGrades)
                    {
                        MessageBox.Show("Cannot delete this component because it has existing grades.");
                        return;
                    }
                }
                components.Remove(comp);
            }
        }

        private void SaveSubject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string subjectName = subjectNameField.Text.Trim();
                if (string.IsNullOrEmpty(subjectName))
                {
                    MessageBox.Show("Subject name cannot be empty.");
                    return;
                }

                var validComponents = components
                    .Where(c => !string.IsNullOrWhiteSpace(c.ComponentName) && c.Weight > 0)
                    .ToList();

                if (validComponents.Count == 0)
                {
                    MessageBox.Show("Please add at least one valid assessment component.");
                    return;
                }

                if (validComponents.Sum(c => c.Weight) != 100)
                {
                    MessageBox.Show("Total weight must equal 100%");
                    return;
                }

                // Cập nhật subject name
                subjectToEdit.SubjectName = subjectName;
                _context.Subjects.Update(subjectToEdit);

                // Lấy component hiện có trong DB
                var existingComponents = _context.AssessmentComponents
                    .Where(c => c.SubjectId == subjectToEdit.SubjectId)
                    .ToList();

                // Xoá component không còn tồn tại trong danh sách hiện tại
                foreach (var oldComp in existingComponents)
                {
                    var stillExists = validComponents.Any(c => c.ComponentId == oldComp.ComponentId);
                    if (!stillExists)
                    {
                        bool hasGrades = _context.Grades.Any(g => g.ComponentId == oldComp.ComponentId);
                        if (hasGrades)
                        {
                            MessageBox.Show($"Cannot delete component '{oldComp.ComponentName}' because it has grades.");
                            return;
                        }
                        _context.AssessmentComponents.Remove(oldComp);
                    }
                }

                // Thêm mới hoặc cập nhật
                foreach (var comp in validComponents)
                {
                    if (comp.ComponentId == 0)
                    {
                        // Thêm mới
                        comp.SubjectId = subjectToEdit.SubjectId;
                        _context.AssessmentComponents.Add(comp);
                    }
                    else
                    {
                        // Cập nhật
                        var existing = _context.AssessmentComponents.FirstOrDefault(c => c.ComponentId == comp.ComponentId);
                        if (existing != null)
                        {
                            existing.ComponentName = comp.ComponentName;
                            existing.Weight = comp.Weight;
                            _context.AssessmentComponents.Update(existing);
                        }
                    }
                }

                _context.SaveChanges();
                MessageBox.Show("Subject updated successfully.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\nInner: {ex.InnerException?.Message}");
            }
        }
    }
}

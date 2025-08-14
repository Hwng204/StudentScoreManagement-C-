using BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddSubject.xaml
    /// </summary>
    public partial class AddSubject : Window
    {
        private ObservableCollection<AssessmentComponent> components = new ObservableCollection<AssessmentComponent>();
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        public AddSubject()
        {
            InitializeComponent();
            componentList.ItemsSource = components;
        }

        private void AddComponent_Click(object sender, RoutedEventArgs e)
        {
            components.Add(new AssessmentComponent { ComponentName = "", Weight = 0 });
        }

        private void SaveSubject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string subjectCode = subjectCodeField.Text.Trim();
                string subjectName = subjectNameField.Text.Trim();

                if (string.IsNullOrEmpty(subjectCode) || string.IsNullOrEmpty(subjectName))
                {
                    MessageBox.Show("Subject Code and Name cannot be empty.");
                    return;
                }
               
                if (_context.Subjects.Any(s => s.SubjectCode == subjectCode))
                {
                    MessageBox.Show("Subject code already exists.");
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

                var duplicateNames = validComponents
                    .GroupBy(c => c.ComponentName.Trim().ToLower())
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicateNames.Any())
                {
                    MessageBox.Show("Component names must be unique.");
                    return;
                }

                if (validComponents.Sum(c => c.Weight) != 100)
                {
                    MessageBox.Show("Total weight must equal 100%");
                    return;
                }

                Subject subject = new Subject
                {
                    SubjectCode = subjectCode,
                    SubjectName = subjectName
                };

                _context.Subjects.Add(subject);
                _context.SaveChanges();

                foreach (var comp in validComponents)
                {
                    comp.SubjectId = subject.SubjectId;
                    _context.AssessmentComponents.Add(comp);
                }

                _context.SaveChanges();

                MessageBox.Show("Subject and components added successfully.");
                this.Close();
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Database error: " + ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message);
            }
        }

    }
}

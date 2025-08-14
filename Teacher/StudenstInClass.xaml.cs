using BusinessLogic.Models;
using BusinessObject.Models;
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
    /// Interaction logic for StudenstInClass.xaml
    /// </summary>
    public partial class StudenstInClass : Window
    {
        private readonly ProjectPrn212Context _context = new();
        private readonly Class selectedClass;
        private List<AssessmentComponent> components;
        private List<StudentScoreEntry> entries = new();
        private BusinessLogic.Models.Teacher getTeacher;

        public StudenstInClass(Class selectedClass, BusinessLogic.Models.Teacher teacher)
        {
            InitializeComponent();
            this.selectedClass = selectedClass;
            classCodeText.Text = selectedClass.ClassCode;
            subjectNameText.Text = selectedClass.Subject.SubjectName;
            getTeacher = teacher;
            LoadAssessmentComponents();
            LoadStudentScores();
            GenerateDynamicColumns();
        }

        private void LoadAssessmentComponents()
        {
            components = _context.AssessmentComponents
                .Where(c => c.SubjectId == selectedClass.SubjectId)
                .ToList();
        }

        private void LoadStudentScores()
        {
            var enrollments = _context.Enrollments
                .Where(e => e.ClassId == selectedClass.ClassId)
                .Include(e => e.Student)
                .ToList();

            foreach (var enroll in enrollments)
            {
                var entry = new StudentScoreEntry
                {
                    StudentId = (int)enroll.StudentId,
                    StudentCode = enroll.Student.StudentCode,
                    FullName = enroll.Student.FullName
                };

                foreach (var comp in components)
                {
                    var score = _context.Grades
                        .Where(g => g.EnrollmentId == enroll.EnrollmentId && g.ComponentId == comp.ComponentId)
                        .Select(g => (double?)g.Score)
                        .FirstOrDefault();

                    entry.Scores[comp.ComponentName] = score;
                }

                entries.Add(entry);
            }

            scoreDataGrid.ItemsSource = entries;
        }

        private void GenerateDynamicColumns()
        {
            foreach (var comp in components)
            {
                var binding = new Binding($"Scores[{comp.ComponentName}]")
                {
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };

                var column = new DataGridTextColumn
                {
                    Header = comp.ComponentName,
                    Binding = binding
                };

                scoreDataGrid.Columns.Add(column);
            }
        }

        private void scoreDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var tb = e.EditingElement as TextBox;
                if (double.TryParse(tb.Text, out double score))
                {
                    if (score < 0 || score > 10)
                    {
                        MessageBox.Show("Score must be between 0 and 10.");
                        tb.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Invalid score.");
                    tb.Text = "";
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            foreach (var entry in entries)
            {
                int enrollmentId = _context.Enrollments
                    .Where(e => e.StudentId == entry.StudentId && e.ClassId == selectedClass.ClassId)
                    .Select(e => e.EnrollmentId)
                    .First();

                foreach (var comp in components)
                {
                    double? score = entry.Scores.ContainsKey(comp.ComponentName) ? entry.Scores[comp.ComponentName] : null;
                    if (score is null) continue;

                    var grade = _context.Grades.FirstOrDefault(g => g.EnrollmentId == enrollmentId && g.ComponentId == comp.ComponentId);
                    if (grade != null)
                    {
                        grade.Score = score;
                        _context.Update(grade);
                    }
                    else
                    {
                        _context.Grades.Add(new Grade
                        {
                            EnrollmentId = enrollmentId,
                            ComponentId = comp.ComponentId,
                            Score = score.Value
                        });
                    }
                }
            }

            _context.SaveChanges();
            MessageBox.Show("Grades saved successfully!");
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ClassView studenstInClass = new ClassView(getTeacher);
            studenstInClass.Show();
            Close();
        }
    }
}

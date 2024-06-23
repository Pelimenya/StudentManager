using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using StudentManager.Context;
using StudentManager.Models;

namespace StudentManager.Pages.UserPages
{
    public partial class CoursesExamsPage : Page
    {
        private List<Course> _courses;
        private List<Exam> _exams;
        private readonly ContextStudentManagerDB _context = new ContextStudentManagerDB();

        public CoursesExamsPage()
        {
            InitializeComponent();
            LoadCourses();
            LoadGroups(); // Загрузка групп при инициализации страницы
        }

        private void LoadCourses()
        {
            _courses = _context.Courses.ToList();
            CoursesDataGrid.ItemsSource = _courses;
        }

        private void LoadGroups()
        {
            var groups = _context.Groups.ToList();
            GroupComboBox.ItemsSource = groups;
        }

        private void CoursesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CoursesDataGrid.SelectedItem != null)
            {
                Course selectedCourse = CoursesDataGrid.SelectedItem as Course;
                if (selectedCourse != null)
                {
                    RefreshExams(selectedCourse.CourseId);
                }
            }
        }

        private void AddCourse_Click(object sender, RoutedEventArgs e)
        {
            Course newCourse = new Course
            {
                CourseName = "Новый предмет",
            };
            _context.Courses.Add(newCourse);
            _context.SaveChanges();
            RefreshCourses();
        }

        private void EditCourse_Click(object sender, RoutedEventArgs e)
        {
            Course selectedCourse = (Course)CoursesDataGrid.SelectedItem;
            if (selectedCourse != null)
            {
                _context.SaveChanges();
                RefreshCourses();
            }
        }

        private void DeleteCourse_Click(object sender, RoutedEventArgs e)
        {
            Course selectedCourse = (Course)CoursesDataGrid.SelectedItem;
            if (selectedCourse != null)
            {
                _context.Courses.Remove(selectedCourse);
                _context.SaveChanges();
                RefreshCourses();
            }
        }

        private void SaveCourseChanges_Click(object sender, RoutedEventArgs e)
        {
            _context.SaveChanges();
            RefreshCourses();
        }

        private void AddExam_Click(object sender, RoutedEventArgs e)
        {
            if (CoursesDataGrid.SelectedItem != null && GroupComboBox.SelectedItem != null)
            {
                Course selectedCourse = (Course)CoursesDataGrid.SelectedItem;
                Group selectedGroup = (Group)GroupComboBox.SelectedItem;

                DateTime selectedDateTime = DateTime.UtcNow;

                Exam newExam = new Exam
                {
                    Grade = "Новый экзамен",
                    ExamDate = selectedDateTime,
                    Groupid = selectedGroup.GroupId,
                    CourseId = selectedCourse.CourseId
                };

                _context.Exams.Add(newExam);
                _context.SaveChanges();
                RefreshExams(selectedCourse.CourseId);
            }
        }

        private void EditExam_Click(object sender, RoutedEventArgs e)
        {
            Exam selectedExam = (Exam)ExamsDataGrid.SelectedItem;
            if (selectedExam != null)
            {
                selectedExam.ExamDate = DateTime.UtcNow; // Update to current UTC time
                _context.SaveChanges();
                RefreshExams(selectedExam.CourseId);
            }
        }

        private void DeleteExam_Click(object sender, RoutedEventArgs e)
        {
            Exam selectedExam = (Exam)ExamsDataGrid.SelectedItem;
            if (selectedExam != null)
            {
                _context.Exams.Remove(selectedExam);
                _context.SaveChanges();
                RefreshExams(selectedExam.CourseId);
            }
        }

        private void SaveExamChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CoursesDataGrid.SelectedItem != null)
                {
                    Course selectedCourse = (Course)CoursesDataGrid.SelectedItem;
                    RefreshExams(selectedCourse.CourseId);
                }
                _context.SaveChanges();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void RefreshCourses()
        {
            _courses = _context.Courses.ToList();
            CoursesDataGrid.ItemsSource = null;
            CoursesDataGrid.ItemsSource = _courses;
        }

        private void RefreshExams(int courseId)
        {
            _exams = _context.Exams.Where(ex => ex.CourseId == courseId).Include(g => g.Group).ToList();
            ExamsDataGrid.ItemsSource = null;
            ExamsDataGrid.ItemsSource = _exams;
        }

        private void GroupComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CoursesDataGrid_SelectionChanged(sender, e);
        }
    }
}
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
    public partial class StudentsPage : Page
    {
        private List<Student> _students;
        private readonly ContextStudentManagerDB _context = new ContextStudentManagerDB();

        public StudentsPage()
        {
            InitializeComponent();
            LoadStudents();
        }

        private void LoadStudents()
        {
            _students = _context.Students.Include(x => x.Identifications).Include(y => y.Contacts).ToList();
            StudentsDataGrid.ItemsSource = _students;
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            // Создаем нового студента
            Student newStudent = new Student
            {
                FullName = "Новый студент",
                GroupId = 1 // Замените это значение на корректное Id группы, к которой принадлежит студент
            };

            _context.Students.Add(newStudent);

            try
            {
                _context.SaveChanges();
                RefreshStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении студента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            Student selectedStudent = (Student)StudentsDataGrid.SelectedItem;
            if (selectedStudent != null)
            {
                _context.SaveChanges();
                RefreshStudents();
            }
        }

        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            Student selectedStudent = (Student)StudentsDataGrid.SelectedItem;
            if (selectedStudent != null)
            {
                _context.Students.Remove(selectedStudent);
                _context.SaveChanges();
                RefreshStudents();
            }
        }

        private void SaveStudentChanges_Click(object sender, RoutedEventArgs e)
        {
            _context.SaveChanges();
            RefreshStudents();
        }

        private void RefreshStudents()
        {
            _students = _context.Students.ToList();
            StudentsDataGrid.ItemsSource = null;
            StudentsDataGrid.ItemsSource = _students;
        }

        private void StudentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Optional: Implement logic if needed
        }
    }
}
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
        private List<Group> _groups;
        private readonly ContextStudentManagerDB _context = new ContextStudentManagerDB();

        public StudentsPage()
        {
            InitializeComponent();
            LoadStudents();
            LoadGroups();
        }

        private void LoadStudents()
        {
            _students = _context.Students.Include(x => x.Identifications).Include(y => y.Contacts).ToList();
            StudentsDataGrid.ItemsSource = _students;
        }

        private void LoadGroups()
        {
            _groups = _context.Groups.ToList();
            GroupComboBox.ItemsSource = _groups;
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            if (GroupComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите группу для студента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Group selectedGroup = (Group)GroupComboBox.SelectedItem;

            Student newStudent = new Student
            {
                FullName = "Новый студент",
                GroupId = selectedGroup.GroupId
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

        private void SaveIdentification_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                Student student = (Student)StudentsDataGrid.SelectedItem;
                if (student != null)
                {
                    Identification identification = button.DataContext as Identification;
                    if (identification != null)
                    {
                        try
                        {
                            _context.SaveChanges();
                            RefreshStudents();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Произошла ошибка при сохранении ИИН: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private void RefreshStudents()
        {
            _students = _context.Students.ToList();
            StudentsDataGrid.ItemsSource = null;
            StudentsDataGrid.ItemsSource = _students;
        }

       
    }
}
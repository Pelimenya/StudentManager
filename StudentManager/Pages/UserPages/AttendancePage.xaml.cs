using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using StudentManager.Context;
using StudentManager.Models;
using StudentManager.ViewModels;

namespace StudentManager.Pages.UserPages
{
    public partial class AttendancePage : Page
    {
        private readonly ContextStudentManagerDB _context = new ContextStudentManagerDB();

        public AttendancePage()
        {
            InitializeComponent();
            AttendanceDatePicker.SelectedDateChanged += AttendanceDatePicker_SelectedDateChanged;
            LoadStudents();
        }

        private void AttendanceDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadStudents();
        }

        private void LoadStudents()
        {
            if (AttendanceDatePicker.SelectedDate == null)
                return;

            var selectedDate = AttendanceDatePicker.SelectedDate.Value.ToUniversalTime();
            var students = _context.Students.ToList();
            var studentAttendances = students.Select(s => new StudentAttendanceViewModel()
            {
                StudentId = s.StudentId,
                FullName = s.FullName,
                IsPresent = _context.Attendances
                    .Any(a => a.StudentId == s.StudentId && a.Date == selectedDate && a.Status == true)
            }).ToList();

            StudentsDataGrid.ItemsSource = studentAttendances;
        }

        private void SaveAttendance_Click(object sender, RoutedEventArgs e)
        {
            if (AttendanceDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedDate = AttendanceDatePicker.SelectedDate.Value.ToUniversalTime();

            foreach (var item in StudentsDataGrid.Items)
            {
                var studentAttendance = (StudentAttendanceViewModel)item;
                var studentId = studentAttendance.StudentId;
                var isPresent = studentAttendance.IsPresent;

                var attendance = _context.Attendances
                    .FirstOrDefault(a => a.StudentId == studentId && a.Date == selectedDate);

                if (attendance != null)
                {
                    attendance.Status = isPresent;
                    _context.Entry(attendance).State = EntityState.Modified;
                }
                else
                {
                    attendance = new Attendance
                    {
                        StudentId = studentId,
                        Date = selectedDate,
                        Status = isPresent
                    };
                    _context.Attendances.Add(attendance);
                }
            }

            _context.SaveChanges();
            MessageBox.Show("Присутствие сохранено успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            // Логика для генерации отчета о присутствии
        }
    }
}
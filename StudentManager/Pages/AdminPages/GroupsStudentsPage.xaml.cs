using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StudentManager.Context;
using StudentManager.Models;

namespace StudentManager.Pages.AdminPages
{
    public partial class GroupsStudentsPage : Page
    {
        private List<Group> _groups;
        private List<Student> _studentsInSelectedGroup;
        private readonly ContextStudentManagerDB _context = new ContextStudentManagerDB();

        public GroupsStudentsPage()
        {
            InitializeComponent();
            LoadGroups();
        }

        private void LoadGroups()
        {
            _groups = _context.Groups.ToList();
            GroupsDataGrid.ItemsSource = _groups;
        }

        private void GroupsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GroupsDataGrid.SelectedItem != null)
            {
                Group selectedGroup = GroupsDataGrid.SelectedItem as Group;
                if (selectedGroup != null)
                {
                    _studentsInSelectedGroup = _context.Students.Where(s => s.GroupId == selectedGroup.GroupId).ToList();
                    StudentsDataGrid.ItemsSource = _studentsInSelectedGroup;
                }
            }
        }

        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            Group newGroup = new Group
            {
                GroupName = "Новая группа",
                CourseNumber = 1 
            };
            _context.Groups.Add(newGroup);
            _context.SaveChanges();
            RefreshGroups();
        }

        private void EditGroup_Click(object sender, RoutedEventArgs e)
        {
            Group selectedGroup = (Group)GroupsDataGrid.SelectedItem;
            if (selectedGroup != null)
            {
                _context.SaveChanges();
                RefreshGroups();
            }
        }

        private void DeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            Group selectedGroup = (Group)GroupsDataGrid.SelectedItem;
            if (selectedGroup != null)
            {
                _context.Groups.Remove(selectedGroup);
                _context.SaveChanges();
                RefreshGroups();
            }
        }

        private void SaveGroupChanges_Click(object sender, RoutedEventArgs e)
        {
            _context.SaveChanges();
            RefreshGroups();
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            if (GroupsDataGrid.SelectedItem != null)
            {
                Group selectedGroup = (Group)GroupsDataGrid.SelectedItem;
                Student newStudent = new Student
                {
                    FullName = "Новый студент",
                    GroupId = selectedGroup.GroupId 
                };
                _context.Students.Add(newStudent);
                _context.SaveChanges();
                RefreshStudents(selectedGroup.GroupId);
            }
        }

        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            Student selectedStudent = (Student)StudentsDataGrid.SelectedItem;
            if (selectedStudent != null)
            {
                _context.SaveChanges();
                RefreshStudents(selectedStudent.GroupId);
            }
        }

        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            Student selectedStudent = (Student)StudentsDataGrid.SelectedItem;
            if (selectedStudent != null)
            {
                _context.Students.Remove(selectedStudent);
                _context.SaveChanges();
                RefreshStudents(selectedStudent.GroupId);
            }
        }

        private void SaveStudentChanges_Click(object sender, RoutedEventArgs e)
        {
            _context.SaveChanges();
            if (GroupsDataGrid.SelectedItem != null)
            {
                Group selectedGroup = (Group)GroupsDataGrid.SelectedItem;
                RefreshStudents(selectedGroup.GroupId);
            }
        }

        private void RefreshGroups()
        {
            _groups = _context.Groups.ToList();
            GroupsDataGrid.ItemsSource = null;
            GroupsDataGrid.ItemsSource = _groups;
        }

        private void RefreshStudents(int groupId)
        {
            _studentsInSelectedGroup = _context.Students.Where(s => s.GroupId == groupId).ToList();
            StudentsDataGrid.ItemsSource = null;
            StudentsDataGrid.ItemsSource = _studentsInSelectedGroup;
        }
    }
}
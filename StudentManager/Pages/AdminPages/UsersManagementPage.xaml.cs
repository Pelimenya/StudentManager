using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading; // Добавляем пространство имен Dispatcher
using Microsoft.EntityFrameworkCore;
using StudentManager.Context;
using StudentManager.Models;

namespace StudentManager.Pages.AdminPages
{
    public partial class UsersManagementPage : Page
    {
        private readonly ContextStudentManagerDB _context = new ContextStudentManagerDB();
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<string> Roles { get; set; }
        public User SelectedUser { get; set; }

        public UsersManagementPage()
        {
            InitializeComponent();
            DataContext = this;
            Users = new ObservableCollection<User>(); // Инициализируем ObservableCollection

            LoadUsers();
            LoadRoles();
        }

        private void LoadUsers()
        {
            Users.Clear(); // Очищаем текущую коллекцию перед загрузкой новых данных
            var users = _context.Users.ToList();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        private void LoadRoles()
        {
            Roles = new ObservableCollection<string> { "admin", "user" }; // Можно загрузить роли из базы данных
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(RoleComboBox.SelectedValue?.ToString()))
                {
                    MessageBox.Show("Необходимо выбрать роль пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newUser = new User
                {
                    Username = UsernameTextBox.Text,
                    Password = PasswordBox.Password,
                    Role = RoleComboBox.SelectedValue?.ToString()
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                LoadUsers();
                ClearInputFields();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedUser != null)
                {
                    if (string.IsNullOrEmpty(RoleComboBox.SelectedValue?.ToString()))
                    {
                        MessageBox.Show("Необходимо выбрать роль пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    SelectedUser.Username = UsernameTextBox.Text;
                    SelectedUser.Password = PasswordBox.Password;
                    SelectedUser.Role = RoleComboBox.SelectedValue?.ToString();

                    _context.Entry(SelectedUser).State = EntityState.Modified;
                    _context.SaveChanges();

                    LoadUsers();
                    ClearInputFields();
                }
                else
                {
                    MessageBox.Show("Выберите пользователя для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedUser != null)
                {
                    _context.Users.Remove(SelectedUser);
                    _context.SaveChanges();

                    LoadUsers();
                    ClearInputFields();
                }
                else
                {
                    MessageBox.Show("Выберите пользователя для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void ClearInputFields()
        {
            UsernameTextBox.Clear();
            PasswordBox.Clear();
            RoleComboBox.SelectedIndex = -1;

            SelectedUser = null;
        }

        private void UsersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedUser = UsersListView.SelectedItem as User;

            if (SelectedUser != null)
            {
                UsernameTextBox.Text = SelectedUser.Username;
                RoleComboBox.SelectedValue = SelectedUser.Role;
            }
        }

        private void RefreshUsers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadUsers(); // Перезагружаем пользователей
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
            Users.Clear(); 
            var users = _context.Users.ToList();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        private void LoadRoles()
        {
            Roles = new ObservableCollection<string> { "admin", "user" }; 
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

                
                string hashedPassword = HashPassword(PasswordBox.Password);

                var newUser = new User
                {
                    Username = UsernameTextBox.Text,
                    Password = hashedPassword,
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

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }
                return builder.ToString();
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
                    SelectedUser.Role = RoleComboBox.SelectedValue?.ToString();

                    
                    if (!string.IsNullOrEmpty(PasswordBox.Password))
                    {
                        SelectedUser.Password = HashPassword(PasswordBox.Password);
                    }

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
                LoadUsers(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
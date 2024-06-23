using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using StudentManager.Context;
using StudentManager.Pages.AdminPages;

namespace StudentManager.Pages.HomePages
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            string enteredLogin = Login.Text;
            string enteredPassword = Password.Password;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedPasswordBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));
                string hashedPassword = BitConverter.ToString(hashedPasswordBytes).Replace("-", "").ToLower();

                using (var context = new ContextStudentManagerDB())
                {
                    var user = context.Users.FirstOrDefault(x => x.Username == enteredLogin);

                    if (user != null && user.Password == hashedPassword)
                    {
                        if (user.Role == "admin")
                        {
                           NavigationService.Navigate(new AdminMenu());
                        }
                        else
                        {
                       //     NavigationService.Navigate(new UserMenu());
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль");
                    }
                }
            }
        }

        
        private void ShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = Password;
            TextBox visiblePasswordTextBox = VisiblePasswordTextBox;
            visiblePasswordTextBox.Text = passwordBox.Password;
            visiblePasswordTextBox.Visibility = Visibility.Visible;
            passwordBox.Visibility = Visibility.Collapsed;
        }

        private void ShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = Password;
            TextBox visiblePasswordTextBox = VisiblePasswordTextBox;
            passwordBox.Password = visiblePasswordTextBox.Text;
            passwordBox.Visibility = Visibility.Visible;
            visiblePasswordTextBox.Visibility = Visibility.Collapsed;
        }
    }
}
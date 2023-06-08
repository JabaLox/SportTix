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
using SportTix.Model;

namespace SportTix.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void passwordCheck(object sender, RoutedEventArgs e)
        {
            Password_boxView.Visibility = Visibility.Visible;
            Password_box.Visibility = Visibility.Hidden;
            Password_boxView.Text = Password_box.Password;
            Password_boxView.Focus();
            Password_boxView.SelectionStart = Password_box.Password.Length;

        }

        private void passwordUnCheck(object sender, RoutedEventArgs e)
        {
            Password_boxView.Visibility = Visibility.Hidden;
            Password_box.Visibility = Visibility.Visible;
            Password_box.Password = Password_boxView.Text;
            Password_box.Focus();
        }

        private void AuthBtnClick(object sender, RoutedEventArgs e)
        {
            Authorization();
        }

        public void Authorization()
        {
            if (!string.IsNullOrEmpty(loginText.Text) && (!string.IsNullOrEmpty(Password_box.Password) || !string.IsNullOrEmpty(Password_boxView.Text)))
            {
                var UserAuth = SporttixContext.Context.Users.FirstOrDefault(x => (x.Password == Password_boxView.Text || x.Password == Password_box.Password) && x.Login == loginText.Text);
                if (UserAuth != null)
                {
                    TicketWindow navigationWindow = new TicketWindow();

                    if (RememberMeCheck.IsChecked == true)
                    {
                        Properties.Settings.Default.RememberMe = true;
                    }
                    Properties.Settings.Default.UserLogin = UserAuth.Login;
                    Properties.Settings.Default.UserPassword = UserAuth.Password;
                    Properties.Settings.Default.IdUser = UserAuth.IdUsers;
                    Properties.Settings.Default.NameUser = UserAuth.Name;
                    Properties.Settings.Default.SurName = UserAuth.SurName;
                    Properties.Settings.Default.Patronomyc = UserAuth.Patronomyc;
                    Properties.Settings.Default.Role = SporttixContext.Context.Roles.FirstOrDefault(x => x.IdRole == UserAuth.Role).NameRole;
                    Properties.Settings.Default.PhotoUser = UserAuth.Photo;
                    Properties.Settings.Default.DateBirthDay = Convert.ToDateTime(Convert.ToDateTime(UserAuth.DateBirth).ToString("dd.MM.yyyy"));
                    Properties.Settings.Default.Save();
                    MessageBox.Show("Авторизация прошла успешно", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    navigationWindow.Show(); this.Close();
                }
                else
                {
                    MessageBox.Show("Логин или пароль введены неверно","Ошибка данных",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Поля не должны быть пустыми", "Пустые поля", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegistrClickLink(object sender, RoutedEventArgs e)
        {
            RegistrationAndEditWindow registrationAndEdit = new RegistrationAndEditWindow(null);
            registrationAndEdit.Show();
            this.Close();
        }

        public bool inputText = false;
        private void loginText_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!inputText)
            {
                loginText.Text = null;
                inputText = true;
                loginText.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF1A1A1A"));
            }
        }

        private void loginText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(loginText.Text))
            {
                loginText.Text = "Введите логин";
                inputText = false;
                loginText.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF777777"));

            }

        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.RememberMe)
            {
                TicketWindow ticketWindow = new TicketWindow();
                ticketWindow.Show();
                this.Close();
            }
        }
    }
}

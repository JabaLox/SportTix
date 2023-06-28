using Firebase.Storage;
using Microsoft.Win32;
using SportTix.Model;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SportTix.Windows
{
    /// <summary>
    /// Логика взаимодействия для RegistrationAndEditWindow.xaml
    /// </summary>
    public partial class RegistrationAndEditWindow : Window
    {
        public int? _IdUser = null;
        public RegistrationAndEditWindow(int? idUser)
        {
            InitializeComponent();
            _IdUser = idUser;
        }
        string Avatar = "";

        private void ViewPasswordCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            passwordTextView.Visibility = Visibility.Hidden;
            passwordText.Visibility = Visibility.Visible;
            passwordText.Password = passwordTextView.Text;

        }

        private void ViewPasswordCheck_Checked(object sender, RoutedEventArgs e)
        {
            passwordTextView.Visibility = Visibility.Visible;
            passwordText.Visibility = Visibility.Hidden;
            passwordTextView.Text = passwordText.Password;
            passwordTextView.Focus();
            passwordTextView.SelectionStart = passwordText.Password.Length;
        }
        //string nameFile = "";
        string fullPathFile = "";
        private void OpenFolderClick(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".png";
            dlg.Filter =
                 "Images (*.png;*.jpg;*.gif;*.jpeg;*.bmp)|" +
                 "*.png;*.jpg;*.gif;*.jpeg;*.bmp|" +
                 "PNG Files (*.png)|*.png|" +
                 "JPEG Files (*.jpeg)|*.jpeg|" +
                 "JPG Files (*.jpg)|*.jpg|" +
                 "GIF Files (*.gif)|*.gif|" +
                 "BMP Files (*.bmp)|*.bmp";
            string imageFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
                ?? throw new InvalidOperationException("Cannot get the system's My Pictures folder path");

            dlg.InitialDirectory = imageFolderPath;

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();


            if (result == true)
            {
                Avatar = dlg.SafeFileName;
                fullPathFile = dlg.FileName;
            }

            if (!string.IsNullOrEmpty(Avatar))
            {
                AvatarImage.ImageSource = new BitmapImage(new Uri(fullPathFile, UriKind.Relative));
            }

        }

        public async void UploadImage()
        {
            var storage = new FirebaseStorage("sporttix-56d51.appspot.com");
            var storageReference = storage.Child(Avatar);
            await storageReference.PutAsync(File.OpenRead(fullPathFile));
        }

        private void RegistrClickBtn(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NameText.Text) && !string.IsNullOrEmpty(SurNameText.Text) && !string.IsNullOrEmpty(PatronomycText.Text) &&
                !string.IsNullOrEmpty(loginText.Text) && (!string.IsNullOrEmpty(passwordText.Password) || !string.IsNullOrEmpty(passwordTextView.Text)) &&
                DateBirthPick.SelectedDate != null)
            {
                int id = SporttixContext.Context.Users.ToList().FindLast(x => x.IdUsers != null).IdUsers + 1;

                User user = new User
                {
                    IdUsers = id,
                    Name = NameText.Text,
                    SurName = SurNameText.Text,
                    Patronomyc = PatronomycText.Text,
                    Login = loginText.Text,
                    Password = passwordText.Password,
                    DateBirth = Convert.ToDateTime(DateBirthPick.SelectedDate.Value.ToString("yyyy-MM-dd")),
                    Photo = Avatar,
                    Role = 1,
                };


                try
                {
                    SporttixContext.Context.Users.Add(user);
                    SporttixContext.Context.SaveChanges();
                    if (!string.IsNullOrEmpty(Avatar))
                        UploadImage();
                    MessageBox.Show("Поздравляем, Вы успешно зарегистрировались!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    AuthWindow auth = new AuthWindow();
                    auth.Show();
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Логин уже имеется", "Ошибка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                    SporttixContext.Context.Remove(user);
                }
            }
            else
            {
                MessageBox.Show("Поля не должны быть пустыми", "Ошибка данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void editUserBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NameText.Text) && !string.IsNullOrEmpty(SurNameText.Text) && !string.IsNullOrEmpty(PatronomycText.Text) && !string.IsNullOrEmpty(loginText.Text)
                && (!string.IsNullOrEmpty(passwordText.Password) && !string.IsNullOrEmpty(passwordTextView.Text)) && !string.IsNullOrEmpty(DateBirthPick.Text))
            {
                var _user = SporttixContext.Context.Users.FirstOrDefault(x => x.IdUsers == _IdUser);

                if (_user != null)
                {
                    try
                    {
                        _user.SurName = SurNameText.Text;
                        _user.Name = NameText.Text;
                        _user.Patronomyc = PatronomycText.Text;
                        _user.DateBirth = Convert.ToDateTime(DateBirthPick.SelectedDate.Value.ToString("yyyy-MM-dd"));
                        if (!string.IsNullOrEmpty(Avatar))
                        {
                            UploadImage();
                            _user.Photo = Avatar;
                            Properties.Settings.Default.PhotoUser = Avatar;
                        }
                        _user.Login = loginText.Text;
                        _user.Password = passwordText.Password;


                        Properties.Settings.Default.SurName = SurNameText.Text;
                        Properties.Settings.Default.NameUser = NameText.Text;
                        Properties.Settings.Default.Patronomyc = PatronomycText.Text;
                        Properties.Settings.Default.DateBirthDay = Convert.ToDateTime(DateBirthPick.SelectedDate.Value.ToString("yyyy-MM-dd"));
                        Properties.Settings.Default.UserLogin = loginText.Text;
                        Properties.Settings.Default.UserPassword = passwordText.Password;

                        SporttixContext.Context.SaveChanges();
                        MessageBox.Show("Профиль успешно отредактирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        TicketWindow ticketWindow = new TicketWindow();
                        ticketWindow.Show();
                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Логин уже имеется", "Ошибка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Поля не должны быть пустыми", "Ошибка данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_IdUser != null)
                // readImage();
                if (GlobalClass.CheckInternetConnection())
                {
                    GlobalClass.readImageStatic(AvatarImage);
                }
                else
                {
                    MessageBox.Show("У вас не имеется интеренет соединение, подключите интернет!");
                    AvatarImage.ImageSource = new BitmapImage(new Uri(GlobalClass.CorrectPath() + "\\Resorces\\Images\\Icons\\NoneAvatar.png"));
                    ImageBtn.IsEnabled = false;

                }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            if (_IdUser == null)
            {
                AuthWindow auth = new AuthWindow();
                auth.Show();
                this.Close();
            }
            else
            {
                TicketWindow ticketWindow = new TicketWindow();
                ticketWindow.Show();
                this.Close();
            }

        }
    }
}

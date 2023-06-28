using Firebase.Storage;
using SportTix.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace SportTix.Windows
{
    /// <summary>
    /// Логика взаимодействия для TicketWindow.xaml
    /// </summary>
    public partial class TicketWindow : Window
    {
        public TicketWindow()
        {
            InitializeComponent();

        }

        public async void readImage()
        {
            string Avatar = "NoneAvatar.png";
            if (!string.IsNullOrEmpty(Properties.Settings.Default.PhotoUser))
                Avatar = Properties.Settings.Default.PhotoUser;
            try
            {
                var storage = new FirebaseStorage("sporttix-56d51.appspot.com");
                var sTOREAGERErEFERENCE = storage.Child(Avatar);

                var downloadUri = await sTOREAGERErEFERENCE.GetDownloadUrlAsync();
                AvatarImage.ImageSource = new BitmapImage(new Uri(downloadUri));
            }
            catch
            {
                MessageBox.Show("У вас не имеется интеренет соединение, подключите интернет!");
                AvatarImage.ImageSource = new BitmapImage(new Uri(GlobalClass.CorrectPath() + "\\Resorces\\Images\\Icons\\NoneAvatar.png"));
            }

        }

        private void ExitLink(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.UserLogin = null;
            Properties.Settings.Default.UserPassword = null;
            Properties.Settings.Default.IdUser = 0;
            Properties.Settings.Default.NameUser = null;
            Properties.Settings.Default.Role = null;
            Properties.Settings.Default.RememberMe = false;
            Properties.Settings.Default.Save();
            AuthWindow auth = new AuthWindow();
            auth.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SporttixContext.Context.Matches.ToList().ForEach(x => SporttixContext.Context.Entry(x).Reload());

            MatchList.ItemsSource = SporttixContext.Context.Matches.OrderBy(x=>x.DateMatch).ToList();

            SetValueComboTeam();

            if (Properties.Settings.Default.Role == "Менеджер")
            {
                ContextMenuList.Visibility = Visibility.Visible;
                AddMatchBtn.Visibility = Visibility.Visible;
                TicketUsersBtn.Visibility = Visibility.Visible;
            }

            if (GlobalClass.CheckInternetConnection())
            {
                GlobalClass.readImageStatic(AvatarImage);
            }
            else
            {
                MessageBox.Show("У вас не имеется интеренет соединение, подключите интернет!");
                AvatarImage.ImageSource = new BitmapImage(new Uri(GlobalClass.CorrectPath() + "\\Resorces\\Images\\Icons\\NoneAvatar.png"));


            }
        }

        public void SetValueComboTeam()
        {
            TeamCombo.Items.Clear();
            var teamList = SporttixContext.Context.Matches.ToList();
            teamList = teamList.DistinctBy(x => x.IdTeamGuest).ToList();
            if (EndDatePick.SelectedDate != null)
            {
                teamList = teamList.Where(x => x.DateMatch >= DateStartPick.SelectedDate && x.DateMatch <= EndDatePick.SelectedDate).ToList();

            }
            TeamCombo.Items.Add("Все команды");
            foreach (var teams in teamList)
            {
                TeamCombo.Items.Add(teams.IdTeamGuestNavigation.NameTeam + " " + teams.IdTeamGuestNavigation.City);
               
            }
        }

        public void RefreshList()
        {
            var matchList = SporttixContext.Context.Matches.OrderBy(x => x.DateMatch).ToList();

            if (TeamCombo.SelectedValue != null)
            {
                if (TeamCombo.SelectedIndex != 0)
                    matchList = matchList.Where(x => x.IdTeamGuestNavigation.NameTeam+" "+x.IdTeamGuestNavigation.City == TeamCombo.SelectedItem.ToString()).ToList();
              
            }

            if (SortCombo.SelectedValue != null)
            {
                switch (SortCombo.SelectedIndex)
                {
                    case 1:
                        matchList = matchList.OrderByDescending(x => x.DateMatch).ToList();
                        break;
                }
            }
            if (StatusCombo.SelectedValue != null)
            {
                if (StatusCombo.SelectedIndex != 0)
                    matchList = matchList.Where(x => x.Status == (StatusCombo.SelectedItem as ComboBoxItem)?.Content.ToString()).ToList();
                MessageBox.Show((StatusCombo.SelectedItem as ComboBoxItem)?.Content.ToString());

            }

            if (DateStartPick.SelectedDate != null)
            {
                matchList = matchList.Where(x => x.DateMatch.Value >= DateStartPick.SelectedDate).ToList();
            }

            if (EndDatePick.SelectedDate != null)
            {
                matchList = matchList.Where(x => x.DateMatch.Value <= EndDatePick.SelectedDate).ToList();
            }

            MatchList.ItemsSource = matchList;
        }

        private void TeamCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshList();
        }

        private void SortCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshList();
        }

        private void DateStartPick_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            EndDatePick.DisplayDateStart = DateStartPick.SelectedDate;
            RefreshList();

        }

        private void EndDatePick_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshList();
            SetValueComboTeam();
        }

        private void HistroryLink(object sender, RoutedEventArgs e)
        {
            HistoryBuyTicketWindow historyBuyTicketWindow = new HistoryBuyTicketWindow();
            historyBuyTicketWindow.Show();
            this.Close();
        }

        private void MenuItemEditClick(object sender, RoutedEventArgs e)
        {
            if (MatchList.SelectedItems != null)
            {
                var items = (dynamic)MatchList.SelectedItem;
                AddEditMatchWindow addEditMatchWindow = new AddEditMatchWindow(items.IdMatches, items.MatchDate);
                addEditMatchWindow.StatusMatchCombo.Text = items.Status;
                addEditMatchWindow.ScoreGuestTeamText.Text = items.ScoreGuestHome.ToString();
                addEditMatchWindow.ScoreHomeTeamText.Text = items.ScoreHomeTeam.ToString();
                addEditMatchWindow.TeamGuestCombo.SelectedIndex = items.IdTeamGuest - 1; ;
                addEditMatchWindow.NameTeamGuestText.Text = items.IdTeamGuestNavigation.NameTeam;
                addEditMatchWindow.EditBtnMatch.Visibility = Visibility.Visible;
                addEditMatchWindow.addBtnMatch.Visibility = Visibility.Hidden;
                addEditMatchWindow.Title = "Редактирование матча";
                addEditMatchWindow.Show();
                this.Close();
            }
        }

        private void MatchList_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MatchList.SelectedItem != null)
            {

                var items = (dynamic)MatchList.SelectedItem;
                if (items.Status == "Не начался")
                {
                    string imgSourcesTeam = items.imageTeam;
                    BuyTicket buyTicket = new BuyTicket();
                    buyTicket.Owner = this;
                    buyTicket.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                    buyTicket.ScoreGuesTeam.Text = items.ScoreHomeTeam.ToString();
                    buyTicket.ScoreHomeTeam.Text = items.ScoreGuestHome.ToString();
                    buyTicket.StatusText.Text = items.Status;
                    buyTicket.DateMatchStartText.Text = items.MatchDate.ToString();
                    buyTicket.TeamGuestText.Text = items.IdTeamGuestNavigation.NameTeam;
                    GlobalClass.MatchId = items.IdMatches;

                    buyTicket.TeamGuestImg.ImageSource = new BitmapImage(new Uri(GlobalClass.CorrectPath() + imgSourcesTeam, UriKind.Relative));
                    buyTicket.ShowDialog();
                }
                else
                {
                    MessageBox.Show($"Матч уже {items.Status.ToLower()}, поэтому вы не можете купить билет");
                }
            }
        }

        private void AddMatchBtn_Click(object sender, RoutedEventArgs e)
        {
            AddEditMatchWindow addEditMatchWindow = new AddEditMatchWindow(null, null);
            addEditMatchWindow.Show();
            this.Close();
        }

        private void EditProfilLink(object sender, RoutedEventArgs e)
        {
            RegistrationAndEditWindow registrationAndEdit = new RegistrationAndEditWindow(Properties.Settings.Default.IdUser);

            registrationAndEdit.NameText.Text = Properties.Settings.Default.NameUser;
            registrationAndEdit.SurNameText.Text = Properties.Settings.Default.SurName;
            registrationAndEdit.PatronomycText.Text = Properties.Settings.Default.Patronomyc;
            registrationAndEdit.DateBirthPick.Text = Properties.Settings.Default.DateBirthDay.ToString();
            registrationAndEdit.loginText.Text = Properties.Settings.Default.UserLogin;
            registrationAndEdit.passwordText.Password = Properties.Settings.Default.UserPassword;
            registrationAndEdit.passwordTextView.Text = Properties.Settings.Default.UserPassword;
            registrationAndEdit.AddUserBtn.Visibility = Visibility.Hidden;
            registrationAndEdit.editUserBtn.Visibility = Visibility.Visible;
            registrationAndEdit.Title = "Редактирования профиля";
            registrationAndEdit.Show(); ;

            this.Close();
        }

        private void MenuItemDeleteClick(object sender, RoutedEventArgs e)
        {
            if (MatchList.SelectedItem != null)
            {
                var items = (dynamic)MatchList.SelectedItem;
                int idMatchDel = items.IdMatches;
                if (SporttixContext.Context.Tickets.FirstOrDefault(x => x.IdMatches == idMatchDel) == null)
                {
                    if (MessageBox.Show("Вы точно хотите удалить этот матч?", "Удаление матча", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        var MathDel = SporttixContext.Context.Matches.FirstOrDefault(x => x.IdMatches == idMatchDel);
                        SporttixContext.Context.Matches.Remove(MathDel);
                        SporttixContext.Context.SaveChanges();
                        MessageBox.Show("Матч успешно удалён");
                        RefreshList();
                    }
                }
                else
                {
                    MessageBox.Show("На этот матч уже приобрели билеты");
                }
            }
        }

        private void TicketUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            TicketUsers ticketUsers = new TicketUsers();
            ticketUsers.Show();
            this.Close();
        }

        private void StatusCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshList();
        }
    }

}

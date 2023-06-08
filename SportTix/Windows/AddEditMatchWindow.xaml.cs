using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddEditMatchWindow.xaml
    /// </summary>
    public partial class AddEditMatchWindow : Window
    {
        public int? _IdMatch = null;
        ///public DateTime? _DateMatch = null;
        public AddEditMatchWindow(int? idMatch, string? dateMatchTime)
        {
            InitializeComponent();
            _IdMatch = idMatch;
            if(dateMatchTime!=null)
                DateTimePick.Text = dateMatchTime.ToString();

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(TeamGuestCombo.SelectedValue!=null)
            {
                var team = SporttixContext
                     .Context.Teams.FirstOrDefault(x => x.IdTeams == TeamGuestCombo.SelectedIndex + 1);
               GuestTeamImage.ImageSource = new BitmapImage(new Uri(GlobalClass.CorrectPath() + @"Resorces\LogoTeams\" + team.Logotype, UriKind.Relative));
                NameTeamGuestText.Text = team.NameTeam;
            }
            else
            {
                GuestTeamImage.ImageSource = new BitmapImage(new Uri(GlobalClass.CorrectPath() + @"Resorces\Images\Icons\picture.png", UriKind.Relative));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TeamGuestCombo.ItemsSource = SporttixContext.Context.Teams.Select(x => x.NameTeam +" "+ x.City).ToList();
            DateTimePick.Watermark = DateTime.Now;
        }

        private void StatusMatchCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(StatusMatchCombo.SelectedIndex!=0)
            {
                NameHomeTeamText.Visibility = Visibility.Visible;
                NameTeamGuestText.Visibility = Visibility.Visible;
                ScoreGuestTeamText.Visibility = Visibility.Visible;
                ScoreHomeTeamText.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TicketWindow ticketWindow = new TicketWindow();
            ticketWindow.Show();
            this.Close();
        }

        private void addBtnMatch_Click(object sender, RoutedEventArgs e)
        {
            if(TeamGuestCombo.SelectedValue!=null && !string.IsNullOrEmpty(DateTimePick.Text))
            {
                int ScoreHome = Convert.ToInt32(ScoreHomeTeamText.Text);
                int ScoreGuest = Convert.ToInt32(ScoreGuestTeamText.Text);
                if (StatusMatchCombo.SelectedIndex==2 && (ScoreHome==0 && ScoreGuest==0)) 
                {
                    MessageBox.Show("В хоккее не может быть ничей");
                }
                else
                {
                  
                    int id = SporttixContext.Context.Matches.ToList().LastOrDefault(x => x.IdMatches != null).IdMatches + 1;
                    Model.Match match = new Model.Match
                    {
                        IdMatches = id,
                        IdTeamGuest = TeamGuestCombo.SelectedIndex + 1,
                        ScoreHomeTeam = ScoreHome,
                        ScoreGuestHome = ScoreGuest,
                        Status = StatusMatchCombo.Text.ToString(),
                        DateMatch = Convert.ToDateTime(DateTimePick.Text),
                };
                    SporttixContext.Context.Matches.Add(match);
                    SporttixContext.Context.SaveChanges();
                    MessageBox.Show("Успех");
                    TicketWindow ticketWindow = new TicketWindow();
                    ticketWindow.Show();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Поля не должны быть пустыми", "Ошибка данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditBtnMatch_Click(object sender, RoutedEventArgs e)
        {
            int ScoreHome = Convert.ToInt32(ScoreHomeTeamText.Text);
            int ScoreGuest = Convert.ToInt32(ScoreGuestTeamText.Text);
            if (StatusMatchCombo.SelectedIndex == 2 && (ScoreHome == 0 && ScoreGuest == 0))
            {
                MessageBox.Show("В хоккее не может быть ничей");
            }
            else
            {
                var matchContext = SporttixContext.Context.Matches.FirstOrDefault(x => x.IdMatches == _IdMatch);
                matchContext.Status = StatusMatchCombo.Text;
                matchContext.DateMatch = Convert.ToDateTime(DateTimePick.Text);
                matchContext.IdTeamGuest = TeamGuestCombo.SelectedIndex + 1;
                matchContext.ScoreGuestHome = ScoreGuest;
                matchContext.ScoreHomeTeam = ScoreHome;
                
                SporttixContext.Context.SaveChanges();
                MessageBox.Show("Матч успешно изменён");

                TicketWindow ticketWindow = new TicketWindow();
               ticketWindow.Show();
                this.Close();
            }
            
        }

        private void ScoreHomeTeamText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string inputSymbol = e.Text.ToString();

            if (!Regex.Match(inputSymbol, @"[0-9]").Success)
            {
                e.Handled = true;
            }
        }

        private void ScoreGuestTeamText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string inputSymbol = e.Text.ToString();

            if (!Regex.Match(inputSymbol, @"[0-9]").Success)
            {
                e.Handled = true;
            }
        }
    }
}

using SportTix.Model;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SportTix.Windows
{
    /// <summary>
    /// Логика взаимодействия для HistoryBuyTicketWindow.xaml
    /// </summary>
    public partial class HistoryBuyTicketWindow : Window
    {
        public HistoryBuyTicketWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HistoryList.ItemsSource = SporttixContext.Context.Usertickets.Where(x => x.IdUser == Properties.Settings.Default.IdUser).ToList();
            SumText.Text = "Общая сумма покупок " + SporttixContext.Context.Usertickets.Where(x => x.IdUser == Properties.Settings.Default.IdUser)
                .Select(p => p.IdTicketNavigation.CostTicket).Sum().ToString()+ " ₽";

            TeamCombo.Items.Add("Все команды");
            foreach (var teams in SporttixContext.Context.Teams.ToList())
            {
                TeamCombo.Items.Add(teams.NameTeam);
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            TicketWindow ticketWindow = new TicketWindow();
            ticketWindow.Show();
            this.Close();
        }

        public void RefreshListHistory()
        {
            var listHistory = SporttixContext.Context.Usertickets.Where(x => x.IdUser == Properties.Settings.Default.IdUser).ToList();

            if (StartDate.SelectedDate != null)
            {
                listHistory = listHistory.Where(x => x.DateBuy >= StartDate.SelectedDate).ToList();
            }

            if (EndDate.SelectedDate != null)
            {
                listHistory = listHistory.Where(x => x.DateBuy <= EndDate.SelectedDate).ToList();
            }

            if (TeamCombo.SelectedValue != null)
            {
                if (TeamCombo.SelectedIndex != 0)
                    listHistory = listHistory.Where(x => x.IdTicketNavigation.IdMatchesNavigation.IdTeamGuest == TeamCombo.SelectedIndex).ToList();
            }

            if (SumComboSort.SelectedValue != null)
            {
                switch (SumComboSort.SelectedIndex)
                {
                    case 0:
                        listHistory = listHistory.OrderBy(x => x.IdTicketNavigation.CostTicket).ToList();
                        break;
                    case 1:
                        listHistory = listHistory.OrderByDescending(x => x.IdTicketNavigation.CostTicket).ToList();
                        break;
                }
            }

            if (SortDateCombo.SelectedValue != null)
            {
                switch (SortDateCombo.SelectedIndex)
                {
                    case 0:
                        listHistory = listHistory.OrderBy(x => x.DateBuy).ToList();
                        break;
                    case 1:
                        listHistory = listHistory.OrderByDescending(x => x.DateBuy).ToList();
                        break;
                }
            }

            HistoryList.ItemsSource = listHistory;
        }

        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            EndDate.DisplayDateStart = StartDate.SelectedDate;
            RefreshListHistory();
        }

        private void EndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshListHistory();
        }

        private void TeamCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshListHistory();
        }

        private void SumComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshListHistory();
        }

        private void SortDateCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshListHistory();
        }
    }
}

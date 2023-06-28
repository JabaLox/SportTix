using SportTix.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SportTix
{
    /// <summary>
    /// Логика взаимодействия для Sector.xaml
    /// </summary>
    public partial class Sector : Page
    {
        public Sector()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateBtnPlaces();
        }

        public void GenerateBtnPlaces()
        {
            RowNumber.Children.Clear();
            PlacesBtnContainer.Children.Clear();

            int CountRow = Convert.ToInt32(GlobalClass.TotalRow);
            int CountPlace = Convert.ToInt32(GlobalClass.TotalPlace);

            PlacesBtnContainer.Width = (int)(21.4285714286 * Convert.ToDouble(GlobalClass.TotalPlace));

            for (int Row = 1; Row <= CountRow; ++Row)
            {

                for (int Place = 1; Place <= CountPlace; ++Place)
                {
                    Button btnPlace = new Button
                    {
                        Tag = Row + " " + Place,
                        Content = Place,

                    };
                    btnPlace.Click += ClickPlace;

                    if (GlobalClass.tickets.Where(x => x.PlaceTicket == Place && x.RowTicket == Row &&
                                                    x.SectorTicket == GlobalClass.SectorPick && x.IdMatchTicket == GlobalClass.MatchId).ToList().Count > 0)
                        btnPlace.Background = Brushes.Gray;

                    if (SporttixContext.Context.Tickets.Where(x => x.Place == Place && x.Row == Row &&
                                                    x.Sector == GlobalClass.SectorPick && x.IdMatches == GlobalClass.MatchId).ToList().Count > 0)
                        btnPlace.IsEnabled = false;

                    PlacesBtnContainer.Children.Add(btnPlace);
                }
                TextBlock TextRow = new TextBlock();
                TextRow.Text = Row.ToString();
                RowNumber.Children.Add(TextRow);
            }
        }

        public void SetTicketList(object sender)
        {
            GlobalClass.tickets.Add(new TicketGlobal
            {
                SectorTicket = GlobalClass.SectorPick,
                IdMatchTicket = GlobalClass.MatchId,
                TypeTicket = GlobalClass.Type,
                CostTicket = GlobalClass.CostSector,
                RowTicket = Convert.ToInt32((sender as Button).Tag.ToString().Split()[0]),
                PlaceTicket = Convert.ToInt32((sender as Button).Tag.ToString().Split()[1]),

            });

        }

        private void ClickPlace(object sender, RoutedEventArgs e)
        {
            //bool TicketAdd = true;

            //if (GlobalClass.tickets.Count > 0)
            //{

            //    for (int i = 0; i < GlobalClass.tickets.Count; ++i)
            //    {
            //        if (GlobalClass.tickets[i].PlaceTicket == Convert.ToInt32((sender as Button).Tag.ToString().Split()[1])
            //             && GlobalClass.tickets[i].RowTicket == Convert.ToInt32((sender as Button).Tag.ToString().Split()[0]))
            //        {
            //            TicketAdd = false;
            //            GlobalClass.tickets.RemoveAt(i);
            //            (sender as Button).Background = Brushes.Purple;
            //        }
            //    }
            //}

            //if (TicketAdd)
            //{
            //    SetTicketList(sender);
            //    (sender as Button).Background = Brushes.Gray;
            //}


            bool TicketAdd = true;

            if (GlobalClass.tickets.Any(ticket => ticket.PlaceTicket == Convert.ToInt32((sender as Button).Tag.ToString().Split()[1])
                                            && ticket.RowTicket == Convert.ToInt32((sender as Button).Tag.ToString().Split()[0])))
            {
                TicketAdd = false;
                GlobalClass.tickets.RemoveAll(ticket => ticket.PlaceTicket == Convert.ToInt32((sender as Button).Tag.ToString().Split()[1])
                                                    && ticket.RowTicket == Convert.ToInt32((sender as Button).Tag.ToString().Split()[0]));
                (sender as Button).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5797ff"));
            }

            if (TicketAdd)
            {
                SetTicketList(sender);

                (sender as Button).Background = Brushes.Gray;
            }

        }

        private void ClosedFrame(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(null);
        }
    }
}

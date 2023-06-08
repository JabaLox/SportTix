using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using SportTix.Model;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Paragraph = MigraDoc.DocumentObjectModel.Paragraph;
using Section = MigraDoc.DocumentObjectModel.Section;

namespace SportTix.Windows
{
    /// <summary>
    /// Логика взаимодействия для BuyTicket.xaml
    /// </summary>
    public partial class BuyTicket : Window
    {
        public BuyTicket()
        {
            InitializeComponent();
        }

        static string path = $"Sector.xaml";
        Uri pageUri = new Uri(path, UriKind.Relative);

        private void SectorBtnClick(object sender, RoutedEventArgs e)
        {

            GlobalClass.TotalRow = (sender as Button).Tag.ToString().Split()[1];
            GlobalClass.TotalPlace = (sender as Button).Tag.ToString().Split()[0];
            GlobalClass.SectorPick = (sender as Button).Tag.ToString().Split()[2];
            GlobalClass.CostSector = Convert.ToInt32((sender as Button).Tag.ToString().Split()[3]);
            GlobalClass.Type = (sender as Button).Tag.ToString().Split()[4];

            BackGrid.Visibility = Visibility;
            GridPlace.Visibility = Visibility;

            FramePlaces.Navigate(pageUri);

            DoubleAnimation BackItemsControl = new DoubleAnimation();
            BackItemsControl.From = 0;
            BackItemsControl.To = 0.5;

            BackItemsControl.Duration = TimeSpan.FromSeconds(1.2);
            BackItemsControl.SpeedRatio = 5;

            BackGrid.BeginAnimation(Grid.OpacityProperty, BackItemsControl);
        }



        private void BackGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            GridPlace.Visibility = Visibility.Hidden;
            BackGrid.Visibility = Visibility.Hidden;

            FramePlaces.NavigationService.Navigate(null);
            var currentPage = FramePlaces.Content as Page;
            currentPage.ShowsNavigationUI = false;

            if (TicketListView.Items.Count == 0)
            {
                SetValueListView();
            }
            else
            {
                TicketListView.Items.Clear();
                SetValueListView();
            }
        }

        public void SetValueListView()
        {
            foreach (var items in GlobalClass.tickets)
            {
                TicketListView.Items.Add(items);
            }
        }

        private void BtnClearListClick(object sender, RoutedEventArgs e)
        {
            if (TicketListView.Items.Count > 0)
                if (MessageBox.Show("Вы уверены?", "Очищение корзины", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    GlobalClass.tickets.Clear();
                    TicketListView.Items.Clear();
                }
        }

        private void DeleteClickListContextMenu(object sender, RoutedEventArgs e)
        {
            if (TicketListView.SelectedItems != null)
            {
                GlobalClass.tickets.RemoveAt(TicketListView.SelectedIndex);
                TicketListView.Items.Clear();
                SetValueListView();
            }
        }

        private void TypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (TypeCombo.SelectedIndex)
            {
                case 0:
                    A1Btn.IsEnabled = true; A2Btn.IsEnabled = true; A3Btn.IsEnabled = true; A4Btn.IsEnabled = true; B1Btn.IsEnabled = true;
                    B2Btn.IsEnabled = true; B3Btn.IsEnabled = true; B4Btn.IsEnabled = true; E1Btn.IsEnabled = true; E2Btn.IsEnabled = true;
                    F1Btn.IsEnabled = true; F2Btn.IsEnabled = true; F3Btn.IsEnabled = true; F4Btn.IsEnabled = true; C1Btn.IsEnabled = true;
                    C2Btn.IsEnabled = true; C3Btn.IsEnabled = true; C4Btn.IsEnabled = true; D1Btn.IsEnabled = true; D2Btn.IsEnabled = true;

                    X1Btn.IsEnabled = true; X2Btn.IsEnabled = true;

                    Y1Btn.IsEnabled = true;
                    break;

                case 1:
                    A1Btn.IsEnabled = true; A2Btn.IsEnabled = true; A3Btn.IsEnabled = true; A4Btn.IsEnabled = true; B1Btn.IsEnabled = true;
                    B2Btn.IsEnabled = true; B3Btn.IsEnabled = true; B4Btn.IsEnabled = true; E1Btn.IsEnabled = true; E2Btn.IsEnabled = true;
                    F1Btn.IsEnabled = true; F2Btn.IsEnabled = true; F3Btn.IsEnabled = true; F4Btn.IsEnabled = true; C1Btn.IsEnabled = true;
                    C2Btn.IsEnabled = true; C3Btn.IsEnabled = true; C4Btn.IsEnabled = true; D1Btn.IsEnabled = true; D2Btn.IsEnabled = true;

                    X1Btn.IsEnabled = false; X2Btn.IsEnabled = false;

                    Y1Btn.IsEnabled = false;
                    break;

                case 2:
                    A1Btn.IsEnabled = false; A2Btn.IsEnabled = false; A3Btn.IsEnabled = false; A4Btn.IsEnabled = false; B1Btn.IsEnabled = false;
                    B2Btn.IsEnabled = false; B3Btn.IsEnabled = false; B4Btn.IsEnabled = false; E1Btn.IsEnabled = false; E2Btn.IsEnabled = false;
                    F1Btn.IsEnabled = false; F2Btn.IsEnabled = false; F3Btn.IsEnabled = false; F4Btn.IsEnabled = false; C1Btn.IsEnabled = false;
                    C2Btn.IsEnabled = false; C3Btn.IsEnabled = false; C4Btn.IsEnabled = false; D1Btn.IsEnabled = false; D2Btn.IsEnabled = false;

                    X1Btn.IsEnabled = true; X2Btn.IsEnabled = true;

                    Y1Btn.IsEnabled = false;
                    break;

                case 3:
                    A1Btn.IsEnabled = false; A2Btn.IsEnabled = false; A3Btn.IsEnabled = false; A4Btn.IsEnabled = false; B1Btn.IsEnabled = false;
                    B2Btn.IsEnabled = false; B3Btn.IsEnabled = false; B4Btn.IsEnabled = false; E1Btn.IsEnabled = false; E2Btn.IsEnabled = false;
                    F1Btn.IsEnabled = false; F2Btn.IsEnabled = false; F3Btn.IsEnabled = false; F4Btn.IsEnabled = false; C1Btn.IsEnabled = false;
                    C2Btn.IsEnabled = false; C3Btn.IsEnabled = false; C4Btn.IsEnabled = false; D1Btn.IsEnabled = false; D2Btn.IsEnabled = false;

                    X1Btn.IsEnabled = false; X2Btn.IsEnabled = false;

                    Y1Btn.IsEnabled = true;
                    break;
            }
        }

        private void BuyClick(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.tickets.Count > 0)
            {
                foreach (var tix in GlobalClass.tickets)
                {
                    int NewTix = 1;
                    if (SporttixContext.Context.Tickets.ToList().Count > 0)
                        NewTix = SporttixContext.Context.Tickets.ToList().FindLast(x => x.IdTicket != null).IdTicket + 1;
                    Ticket tick = new Ticket
                    {
                        IdTicket = NewTix,
                        CostTicket = tix.CostTicket,
                        IdMatches = tix.IdMatchTicket,
                        Place = tix.PlaceTicket,
                        Sector = tix.SectorTicket,
                        Row = tix.RowTicket,
                        IdTypeTicket = Convert.ToInt32(tix.TypeTicket),
                    };
                    SporttixContext.Context.Tickets.Add(tick);
                    SporttixContext.Context.SaveChanges();
                    int idTix = SporttixContext.Context.Tickets.ToList().FindLast(x => x.IdTicket != null).IdTicket;

                    Userticket userticket = new Userticket
                    {
                        IdTicket = idTix,
                        IdUser = Properties.Settings.Default.IdUser,
                        DateBuy = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm")),
                    };
                    SporttixContext.Context.Add(userticket);
                    SporttixContext.Context.SaveChanges();
                }
                GenerateCheckPDF();
                GlobalClass.tickets.Clear();
                TicketListView.Items.Clear();

            }

        }

        public void GenerateCheckPDF()
        {
            int SumCost = 0;

            Document document = new Document();
            Section section = document.AddSection();
            section.PageSetup.PageFormat = PageFormat.A4;//стандартный размер страницы

            section.PageSetup.BottomMargin = 10;//нижний отступ
            section.PageSetup.TopMargin = 10;//верхний отступ

            Paragraph paragraph = new Paragraph();
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            section.Add(paragraph);
            paragraph.AddImage(GlobalClass.CorrectPath() + @"Resorces\Images\logotype\SportTixLogo.jpg").Width = 250;

            //paragraph.AddImage(@"C:\Users\yrok0\source\repos\SportTix\SportTix\Resorces\Images\logotype\SportTixLogo.jpg");
            MigraDoc.DocumentObjectModel.Font font = new MigraDoc.DocumentObjectModel.Font();
            font.Bold = true;
            font.Size = 15;


            paragraph.AddFormattedText("\n 'ООО SportTix'", font);
            paragraph.AddFormattedText($"\n Покупатель: {Properties.Settings.Default.NameUser}", font); ;
            paragraph.AddFormattedText($"\n Логин покупателя: {Properties.Settings.Default.UserLogin}", font); ;
            paragraph.AddFormattedText($"\nМатч: Салават Юлаев vs {TeamGuestText.Text} в " +
                $"{SporttixContext.Context.Matches.FirstOrDefault(x => x.IdMatches == GlobalClass.MatchId).DateMatch.Value.ToString("dd.MM.yyyy HH:mm")} " + "\n", font);//текст
            font.Size = 10;
            foreach (var Tix in GlobalClass.tickets)
            {
                paragraph.AddText($"Сектор: "); paragraph.AddFormattedText($"{Tix.SectorTicket} ", font);
                paragraph.AddText($"Ряд: "); paragraph.AddFormattedText($"{Tix.RowTicket} ", font);
                paragraph.AddText($"Место: "); paragraph.AddFormattedText($"{Tix.PlaceTicket} ", font);
                paragraph.AddText($"Тип: "); paragraph.AddFormattedText($"{SporttixContext.Context.Typetickets.
                    FirstOrDefault(x => x.IdTypeTicket == Convert.ToInt32(Tix.TypeTicket)).NameTypeTicket} Edition ", font);
                paragraph.AddText($"Цена: "); paragraph.AddFormattedText($"{Tix.CostTicket} ₽\n", font);
                SumCost += Tix.CostTicket;

            }
            font.Size = 15;
            paragraph.AddText("\n");
            paragraph.AddFormattedText($"Общая сумма: {SumCost} ₽\n", font);
            paragraph.AddFormattedText($"Дата и время покупки: {DateTime.Now}", font);
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true, PdfFontEmbedding.Always);
            pdfRenderer.Document = document;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            pdfRenderer.RenderDocument();
            pdfRenderer.PdfDocument.Save(GlobalClass.CorrectPath() + @"\Resorces\PdfFile\SportTixCheck.pdf");// сохраняем

            var p = new Process();
            p.StartInfo = new ProcessStartInfo(GlobalClass.CorrectPath() + @"\Resorces\PdfFile\SportTixCheck.pdf")
            {
                UseShellExecute = true
            };
            p.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (GlobalClass.tickets != null)
            {
                if (MessageBox.Show("Если вы закроете таблицу, то обнулится корзина, Вы уверены?", "Закрытия окна", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    GlobalClass.tickets.Clear();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }

}

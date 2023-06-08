using SportTix.Model;
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
using Paragraph = MigraDoc.DocumentObjectModel.Paragraph;
using Section = MigraDoc.DocumentObjectModel.Section;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System.Diagnostics;
using MigraDoc.DocumentObjectModel.Tables;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SportTix.Windows
{
    /// <summary>
    /// Логика взаимодействия для TicketUsers.xaml
    /// </summary>
    public partial class TicketUsers : Window
    {
        public TicketUsers()
        {
            InitializeComponent();
        }
        public List<Userticket> usertickets = SporttixContext.Context.Usertickets.ToList();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BuyUsersList.ItemsSource = usertickets;
            SumText.Text = "Общая сумма покупок " + SporttixContext.Context.Usertickets
                .Select(p => p.IdTicketNavigation.CostTicket).Sum().ToString();

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

        public void RefreshListBuy()
        {
            var listHistory = usertickets.ToList();

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

            BuyUsersList.ItemsSource = listHistory;
        }

        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            EndDate.DisplayDateStart = StartDate.SelectedDate;
            RefreshListBuy();
        }

        private void EndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshListBuy();
        }

        private void TeamCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshListBuy();
        }

        private void SumComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshListBuy();
        }

        private void SortDateCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshListBuy();
        }

        private void ViewReportBtnClick(object sender, RoutedEventArgs e)
        {
            GenerateCheckPDF();
        }

        public void GenerateCheckPDF()
        {
            var listUsers = SporttixContext.Context.Usertickets.ToList();
            int? stonks = null;
            int? countBuy = null;
            string[] months = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" }; 

            Document document = new Document();

            Section section = document.AddSection();

            Paragraph paragraph = new Paragraph();
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            section.Add(paragraph);
            MigraDoc.DocumentObjectModel.Font font = new MigraDoc.DocumentObjectModel.Font();
            font.Bold = true;
            font.Size = 13;
            paragraph.AddImage(GlobalClass.CorrectPath() + @"Resorces\Images\logotype\SportTixLogo.jpg").Width = 125;
            paragraph.AddFormattedText("\nОтчёт по продажам билетов за каждый месяц", font);
            paragraph.AddFormattedText("\nОбщая сумма продаж: " + listUsers.Select(x => x.IdTicketNavigation.CostTicket).Sum().ToString()+"\n", font);
            MigraDoc.DocumentObjectModel.Tables.Table table = section.AddTable();

            table.AddColumn(Unit.FromCentimeter(5));
            table.AddColumn(Unit.FromCentimeter(7));
            table.AddColumn(Unit.FromCentimeter(9));

            Row headerRow = table.AddRow();
            headerRow.Shading.Color = MigraDoc.DocumentObjectModel.Colors.LightGray; 

            headerRow.Cells[0].AddParagraph("Месяц");
            headerRow.Cells[1].AddParagraph("Сумма продаж");
            headerRow.Cells[2].AddParagraph("Кол-во продаж");

            for (int i = 0; i < months.Length; i++)
            {
                string month = months[i];

                countBuy = listUsers.Where(x => x.DateBuy.Value.Date.Month == i + 1).Count();
                stonks = listUsers
                    .Where(x => x.DateBuy.Value.Date.Month == i + 1)
                    .Select(x => x.IdTicketNavigation.CostTicket)
                    .Sum();
                Debug.WriteLine($"{listUsers.FirstOrDefault(x => x.IdTicket != null).DateBuy.Value.Date.Month} {i + 1}");
                if (stonks != 0)
                {
                    Row dataRow = table.AddRow();
                    dataRow.Cells[0].AddParagraph(month);
                    dataRow.Cells[1].AddParagraph(stonks.ToString());
                    dataRow.Cells[2].AddParagraph(countBuy.ToString());
                }
            }

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true, PdfFontEmbedding.Always);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            renderer.Document = document;
            renderer.RenderDocument();
            renderer.PdfDocument.Save(GlobalClass.CorrectPath() + @"Resorces\PdfFile\ReportSportTix.pdf");
           
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(GlobalClass.CorrectPath() + @"Resorces\PdfFile\ReportSportTix.pdf")
            {
                UseShellExecute = true
            };
            p.Start();
        }
    }
}

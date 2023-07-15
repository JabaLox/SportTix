using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SportTix
{
    public static class GlobalClass
    {
        public static string TotalRow { get; set; }
        public static string TotalPlace { get; set; }
        public static string SectorPick { get; set; }
        public static int CostSector { get; set; }
        public static string Type { get; set; }
        public static int MatchId { get; set; }

        public static List<TicketGlobal> tickets = new List<TicketGlobal>();


        public static string CorrectPath()
        {
            string path = Directory.GetCurrentDirectory();
            int id = path.IndexOf("bin");
            path = path.Remove(id, 24);
            return path;
        }

        public static string Avatar = "";
        public static async void readImageStatic(ImageBrush img)
        {

            if (!string.IsNullOrEmpty(Properties.Settings.Default.PhotoUser))
                Avatar = Properties.Settings.Default.PhotoUser;
            else
                Avatar = "NoneAvatar.png";

            var storage = new FirebaseStorage("sporttix-56d51.appspot.com");
            var sTOREAGERErEFERENCE = storage.Child(Avatar);

            var downloadUri = await sTOREAGERErEFERENCE.GetDownloadUrlAsync();
            img.ImageSource = new BitmapImage(new Uri(downloadUri));

        }

        public static bool CheckInternetConnection()
        {
            try
            {
                var ping = new Ping();
                var result = ping.Send("8.8.8.8", 1000);
                return result.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }

    }

    public class TicketGlobal
    {
        public int RowTicket { get; set; }
        public int PlaceTicket { get; set; }
        public string SectorTicket { get; set; }
        public string TypeTicket { get; set; }
        public int CostTicket { get; set; }
        public int IdMatchTicket { get; set; }

    }
}

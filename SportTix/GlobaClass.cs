 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

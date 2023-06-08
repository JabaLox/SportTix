using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportTix.Model
{
     public partial class Match
    {
        public string MatchDate { get { return DateMatch.Value.ToString("dd.MM.yyyy HH:mm"); } }
        public string imageTeam { get { return IdTeamGuestNavigation.Logotype != null ? $@"\Resorces\LogoTeams\{IdTeamGuestNavigation.Logotype}" : null; } }
    }

    public partial class Userticket
    {
        public string imageTeam { get { return IdTicketNavigation.IdMatchesNavigation.IdTeamGuestNavigation.Logotype != null 
                    ? $@"\Resorces\LogoTeams\{IdTicketNavigation.IdMatchesNavigation.IdTeamGuestNavigation.Logotype}" : null; } }

        public string BuyDate { get { return DateBuy.Value.ToString("dd.MM.yyyy HH:mm"); } }
    }
}

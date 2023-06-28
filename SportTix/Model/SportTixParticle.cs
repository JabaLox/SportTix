namespace SportTix.Model
{
    public partial class Match
    {
        public string MatchDate { get { return DateMatch.Value.ToString("dd.MM.yyyy HH:mm"); } }
        public string imageTeam { get { return IdTeamGuestNavigation.Logotype != null ? $@"\Resorces\LogoTeams\{IdTeamGuestNavigation.Logotype}" : null; } }

        public string ColorStatus
        {
            get
            {
                if (Status == "Не начался")
                    return "#FF13FF00";
                else if (Status == "Идёт")
                    return "#f2f200";
                else
                    return "#ed1307";
            }
        }
    }

    public partial class Userticket
    {
        public string imageTeam
        {
            get
            {
                return IdTicketNavigation.IdMatchesNavigation.IdTeamGuestNavigation.Logotype != null
                    ? $@"\Resorces\LogoTeams\{IdTicketNavigation.IdMatchesNavigation.IdTeamGuestNavigation.Logotype}" : null;
            }
        }

        public string BuyDate { get { return DateBuy.Value.ToString("dd.MM.yyyy HH:mm"); } }
    }


}

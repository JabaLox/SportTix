using System;
using System.Collections.Generic;

namespace SportTix.Model;

public partial class Match
{
    public int IdMatches { get; set; }

    public int? IdTeamGuest { get; set; }

    public DateTime? DateMatch { get; set; }

    public int? ScoreHomeTeam { get; set; }

    public int? ScoreGuestHome { get; set; }

    public string Status { get; set; }

    public virtual Team IdTeamGuestNavigation { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}

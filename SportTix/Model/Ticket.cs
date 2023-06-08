using System;
using System.Collections.Generic;

namespace SportTix.Model;

public partial class Ticket
{
    public int IdTicket { get; set; }

    public int? IdMatches { get; set; }

    public int? CostTicket { get; set; }

    public int? IdTypeTicket { get; set; }

    public string Sector { get; set; }

    public int? Row { get; set; }

    public int? Place { get; set; }

    public virtual Match IdMatchesNavigation { get; set; }

    public virtual Typeticket IdTypeTicketNavigation { get; set; }

    public virtual ICollection<Userticket> Usertickets { get; set; } = new List<Userticket>();
}

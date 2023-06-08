using System;
using System.Collections.Generic;

namespace SportTix.Model;

public partial class Typeticket
{
    public int IdTypeTicket { get; set; }

    public string NameTypeTicket { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}

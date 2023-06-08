using System;
using System.Collections.Generic;

namespace SportTix.Model;

public partial class Userticket
{
    public int IdUser { get; set; }

    public int IdTicket { get; set; }

    public DateTime? DateBuy { get; set; }

    public virtual Ticket IdTicketNavigation { get; set; }

    public virtual User IdUserNavigation { get; set; }
}

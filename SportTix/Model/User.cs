using System;
using System.Collections.Generic;

namespace SportTix.Model;

public partial class User
{
    public int IdUsers { get; set; }

    public string Name { get; set; }

    public string SurName { get; set; }

    public string Patronomyc { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public DateTime? DateBirth { get; set; }

    public string Photo { get; set; }

    public int? Role { get; set; }

    public virtual Role RoleNavigation { get; set; }

    public virtual ICollection<Userticket> Usertickets { get; set; } = new List<Userticket>();
}

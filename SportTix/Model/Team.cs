using System.Collections.Generic;

namespace SportTix.Model;

public partial class Team
{
    public int IdTeams { get; set; }

    public string NameTeam { get; set; }

    public string City { get; set; }

    public string Logotype { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
}

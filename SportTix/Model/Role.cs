using System.Collections.Generic;

namespace SportTix.Model;

public partial class Role
{
    public int IdRole { get; set; }

    public string NameRole { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

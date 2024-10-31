using System;
using System.Collections.Generic;

namespace KahaTiev.Data.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

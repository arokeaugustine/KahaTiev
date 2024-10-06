using System;
using System.Collections.Generic;

namespace KahaTiev.Models;

public partial class Product
{
    public int Id { get; set; }

    public Guid? Guid { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime DateCreated { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();
}

using System;
using System.Collections.Generic;

namespace KahaTiev.Models;

public partial class Package
{
    public int Id { get; set; }

    public Guid? Guid { get; set; }

    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Amount { get; set; }

    public bool IsActive { get; set; }

    public DateTime DateCreated { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Product Product { get; set; } = null!;
}

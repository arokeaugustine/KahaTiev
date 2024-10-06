using System;
using System.Collections.Generic;

namespace KahaTiev.Models;

public partial class User
{
    public int Id { get; set; }

    public Guid? Guid { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Role Role { get; set; } = null!;
}

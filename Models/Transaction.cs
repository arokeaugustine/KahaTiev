using System;
using System.Collections.Generic;

namespace KahaTiev.Models;

public partial class Transaction
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Amount { get; set; }

    public string TransactionReference { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsSuccessful { get; set; }
}

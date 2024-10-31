namespace KahaTiev.Data.DTOs.Payment
{
    public class PaymentViewModel
    {
        public Guid? packageGuid { get; set; }

        public string PackageName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal PackageAmount { get; set; }
        public string PayerName { get; set; } = string.Empty;
        public string PayerEmail { get; set; } = string.Empty;
        public int Amount { get; set; }

    }
}

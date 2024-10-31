namespace KahaTiev.Data.DTOs
{
    public class PackageDTO
    {
        public Guid? Guid { get; set; }

        public int ProductId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public bool IsActive { get; set; }
    }
}

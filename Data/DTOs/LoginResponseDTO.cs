namespace KahaTiev.Data.DTOs
{
    public class LoginResponseDTO
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}

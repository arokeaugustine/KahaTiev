namespace KahaTiev.Data.DTOs
{
    public class MailDTO
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string BlindCopy { get; set; }
        public string Copy { get; set; }
    }
}

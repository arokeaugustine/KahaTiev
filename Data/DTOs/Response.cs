namespace KahaTiev.Data.DTOs
{
    public class Response
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class DataResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}

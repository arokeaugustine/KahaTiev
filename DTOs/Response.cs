namespace KahaTiev.DTOs
{
    public class Response
    {
        public bool status { get; set; }
        public string message { get; set; }
    }

    public class DataResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}

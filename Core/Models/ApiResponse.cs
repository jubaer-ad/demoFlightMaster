namespace Core.Models
{
    public class ApiResponse
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public string Data { get; set; }
        public DateTime Timestamp { get; set; }
        public TimeSpan ProcessingTime { get; set; }
    }
}

namespace MinimalChatAPI.Models.Domain
{
    public class LogFilter
    {
        public DateTime StartTime { get; set; } = DateTime.UtcNow.AddMinutes(-5);
        public DateTime EndTime { get; set; } = DateTime.UtcNow;
    }
}

using Microsoft.AspNetCore.Mvc;

namespace MinimalChatAPI.Models.DTO
{
    public class ConversationHistoryRequestDto
    {
        public int UserId { get; set; }
        public DateTime? Before { get; set; } = DateTime.UtcNow; // Default value is set to current UTC timestamp
        public int Count { get; set; } = 20; // Default value is set here
        public string Sort { get; set; } = "asc"; // Default value is set here
    }
}

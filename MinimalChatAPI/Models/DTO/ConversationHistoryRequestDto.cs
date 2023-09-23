using System;
using Microsoft.AspNetCore.Mvc;

namespace MinimalChatAPI.Models.DTO
{
    public class ConversationHistoryRequestDto
    {
        public int UserId { get; set; }
        public DateTime? Before { get; set; }
        public int Count { get; set; } 
        public string Sort { get; set; } 
    }
}

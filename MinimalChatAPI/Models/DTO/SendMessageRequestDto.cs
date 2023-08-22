namespace MinimalChatAPI.Models.DTO
{
    public class SendMessageRequestDto
    {
        public int ReceiverId { get; set; }
        public string Content { get; set; }
    }
}

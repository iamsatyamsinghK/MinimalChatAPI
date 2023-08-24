using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalChatAPI.Models.Domain
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Sender")]
        [Required]
        public int SenderId { get; set; }

        [ForeignKey("Receiver")]
        [Required]
        public int ReceiverId { get; set; }

        [Required]
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}

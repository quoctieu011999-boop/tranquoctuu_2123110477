using System.ComponentModel.DataAnnotations;
namespace tranquoctuu_2123110477.Models;

    public class CustomerInteraction
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        public string InteractionType { get; set; } // Call, Email, Chat
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Customer Customer { get; set; }
    }


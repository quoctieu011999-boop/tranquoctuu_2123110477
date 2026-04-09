using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace tranquoctuu_2123110477.Models
{
    public class CustomerChannel
    {
        [Key] 
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string? ChannelType { get; set; } 

        public string? ExternalId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
namespace tranquoctuu_2123110477.Models;
public class CustomerChannel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string ChannelType { get; set; }
        public string ExternalId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Customer Customer { get; set; }
    }


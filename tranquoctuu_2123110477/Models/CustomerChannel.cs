using System.ComponentModel.DataAnnotations;

namespace tranquoctuu_2123110477.Models
{
    public class CustomerChannel : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string ChannelType { get; set; }
        public string ExternalId { get; set; }

     
        public Customer? Customer { get; set; }
    }
}
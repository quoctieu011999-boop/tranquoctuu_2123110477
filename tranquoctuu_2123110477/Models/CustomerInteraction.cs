using System.ComponentModel.DataAnnotations;

namespace tranquoctuu_2123110477.Models
{
    public class CustomerInteraction : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string InteractionType { get; set; } 
        public string Content { get; set; }

        
        public Customer? Customer { get; set; }
    }
}
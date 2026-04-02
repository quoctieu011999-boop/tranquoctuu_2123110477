using System.ComponentModel.DataAnnotations;

namespace tranquoctuu_2123110477.Models
{
    public class Order : BaseEntity
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public decimal TotalAmount { get; set; } = 0;

        public string Status { get; set; } 

        public bool IsDeleted { get; set; } = false;

        
        public Customer Customer { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
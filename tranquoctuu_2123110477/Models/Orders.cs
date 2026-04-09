using System.ComponentModel.DataAnnotations;
namespace tranquoctuu_2123110477.Models;

public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Channel { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Customer Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }


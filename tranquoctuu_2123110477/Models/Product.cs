using System.ComponentModel.DataAnnotations;

namespace tranquoctuu_2123110477.Models
{
    public class Product : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; } = false;

        
        public List<OrderItem>? OrderItems { get; set; }
    }
}
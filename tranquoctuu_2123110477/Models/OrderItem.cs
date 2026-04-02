using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tranquoctuu_2123110477.Models
{
    public class OrderItem : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải ít nhất là 1")]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } 

       
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
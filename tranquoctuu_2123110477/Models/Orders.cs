using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tranquoctuu_2123110477.Models
{
    public class Order : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } = 0;

        [Required]
        [StringLength(50)]
        public string Channel { get; set; } 

        [Required]
        [StringLength(50)]
        public string Status { get; set; } 

        // Quan hệ Navigation
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        public virtual List<OrderItem>? OrderItems { get; set; }
    }
}
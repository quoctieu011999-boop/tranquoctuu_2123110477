using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tranquoctuu_2123110477.Models
{
    public class Product : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string Description { get; set; }
        public string? Image { get; set; }


        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
        
        public List<OrderItem>? OrderItems { get; set; }
    }
}
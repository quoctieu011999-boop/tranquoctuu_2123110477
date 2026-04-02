using System.ComponentModel.DataAnnotations;

namespace tranquoctuu_2123110477.Models
{
    public class Category : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(255)]
        public string Name { get; set; }

        public string? Description { get; set; }

        // Quan hệ 1 danh mục có nhiều sản phẩm
        public virtual List<Product>? Products { get; set; }
    }
}
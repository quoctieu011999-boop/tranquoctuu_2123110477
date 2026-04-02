using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tranquoctuu_2123110477.Models
{
    public class CustomerInteraction : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        public string InteractionType { get; set; } 

        [Required]
        public string Content { get; set; } 

        // Khai báo khóa ngoại rõ ràng
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
    }
}
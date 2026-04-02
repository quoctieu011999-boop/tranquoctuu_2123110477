using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tranquoctuu_2123110477.Models
{
    public class Payment : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(50)]
        public string Method { get; set; } 

        [Required]
        [StringLength(50)]
        public string Status { get; set; } 

        // Navigation property
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }
    }
}
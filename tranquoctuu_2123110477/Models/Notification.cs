using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tranquoctuu_2123110477.Models
{
    public class Notification : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; } 

        [StringLength(50)]
        public string Type { get; set; } 

        public bool IsSent { get; set; } = false; 

        public bool IsRead { get; set; } = false; 

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
    }
}
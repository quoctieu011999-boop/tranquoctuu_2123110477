using System.ComponentModel.DataAnnotations;

namespace tranquoctuu_2123110477.Models
{
    public class Customer : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên khách hàng là bắt buộc")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(15)]
        public string Phone { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        public DateTime? Birthday { get; set; }

        [StringLength(20)]
        public string Level { get; set; } 

        public int Points { get; set; } = 0;

        
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
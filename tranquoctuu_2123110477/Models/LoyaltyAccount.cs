using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tranquoctuu_2123110477.Models
{
    public class LoyaltyAccount : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        
        public int TotalPoints { get; set; } = 0;

        [StringLength(20)]
        public string Level { get; set; } = "Normal"; 

     

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
    }
}
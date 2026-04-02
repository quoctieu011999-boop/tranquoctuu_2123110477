using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tranquoctuu_2123110477.Models
{
    public class CustomerChannel : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        public string ChannelType { get; set; } 

        [StringLength(255)]
        public string ExternalId { get; set; }

        
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
    }
}
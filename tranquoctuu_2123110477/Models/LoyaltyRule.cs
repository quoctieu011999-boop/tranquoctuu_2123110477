using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tranquoctuu_2123110477.Models
{
    public class LoyaltyRule : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string RuleName { get; set; } 

        public string? Description { get; set; }

        [Required]
        public int Points { get; set; } 

        [Required]
        [StringLength(50)]
        public string ActionType { get; set; } 

        public decimal? ConditionValue { get; set; } 

        public bool IsActive { get; set; } = true; 
    }
}
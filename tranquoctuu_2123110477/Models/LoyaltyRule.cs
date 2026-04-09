using System.ComponentModel.DataAnnotations;

namespace tranquoctuu_2123110477.Models
{
    public class LoyaltyRule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int PointsReward { get; set; }

        public decimal ConditionAmount { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;
    }
}
using System.ComponentModel.DataAnnotations;

namespace tranquoctuu_2123110477.Models
{
    public class Redemption : BaseEntity
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public int RewardId { get; set; }

        public int PointsUsed { get; set; }

        public string Status { get; set; }

        
        public DateTime RedeemedAt { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        public Customer? Customer { get; set; }
        public Reward? Reward { get; set; }
    }
}
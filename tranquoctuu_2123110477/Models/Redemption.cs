using System.ComponentModel.DataAnnotations;
namespace tranquoctuu_2123110477.Models;

    public class Redemption
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int RewardId { get; set; }
        public int PointsUsed { get; set; }
        public string Status { get; set; }

        public Customer Customer { get; set; }
        public Reward Reward { get; set; }
    }


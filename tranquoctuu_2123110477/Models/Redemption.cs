using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // 1. Thêm thư viện này

namespace tranquoctuu_2123110477.Models
{
    public class Redemption : BaseEntity
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public int RewardId { get; set; }

        public int PointsUsed { get; set; }

        public string? Status { get; set; } // Nên thêm ? cho string

        public DateTime RedeemedAt { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        [JsonIgnore] // 2. Thêm cái này để Swagger không bắt nhập dữ liệu Customer
        public virtual Customer? Customer { get; set; }

        [JsonIgnore] // 3. Thêm cái này để Swagger không bắt nhập dữ liệu Reward
        public virtual Reward? Reward { get; set; }
    }
}
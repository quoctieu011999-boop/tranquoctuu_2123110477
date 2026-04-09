using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // 1. Thêm dòng này để dùng được JsonIgnore

namespace tranquoctuu_2123110477.Models
{
    public class Reward : BaseEntity
    {
        public int Id { get; set; }

        public string? Name { get; set; } 

        public string? Description { get; set; } 

        public int PointCost { get; set; }

        public int Quantity { get; set; } = 0;

        public bool IsDeleted { get; set; } = false;

        [JsonIgnore] 
        public virtual List<Redemption>? Redemptions { get; set; } 
    }
}
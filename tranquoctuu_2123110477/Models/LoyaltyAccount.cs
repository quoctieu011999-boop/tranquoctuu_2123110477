using System.ComponentModel.DataAnnotations;

namespace tranquoctuu_2123110477.Models
{
    public class LoyaltyAccount : BaseEntity
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int Points { get; set; }

        public string Level { get; set; }

        public bool IsDeleted { get; set; } = false;

        public Customer? Customer { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
namespace tranquoctuu_2123110477.Models;

    public class LoyaltyTransaction
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int Points { get; set; }
        public string Type { get; set; }
        public int? ReferenceId { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Customer Customer { get; set; }
   }

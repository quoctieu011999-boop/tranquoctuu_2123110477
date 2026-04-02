using System.ComponentModel.DataAnnotations;

namespace tranquoctuu_2123110477.Models
{
    public class Payment : BaseEntity
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public string Method { get; set; } 

        public string Status { get; set; }

        public bool IsDeleted { get; set; } = false;

     
        public Order? Order { get; set; }
    }
}
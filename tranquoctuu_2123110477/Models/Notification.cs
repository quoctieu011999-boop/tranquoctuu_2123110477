using System.ComponentModel.DataAnnotations;

namespace tranquoctuu_2123110477.Models
{
    public class Notification : BaseEntity
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }

        public bool IsSent { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

      
        public Customer? Customer { get; set; }
    }
}
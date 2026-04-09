using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tranquoctuu_2123110477.Models
{
    // BẠN CẦN THÊM DÒNG NÀY ĐỂ ĐỊNH NGHĨA LỚP:
    public class LoyaltyAccount
    {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int TotalPoints { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        
        public bool IsDeleted { get; set; } = false;

     
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
    }
}
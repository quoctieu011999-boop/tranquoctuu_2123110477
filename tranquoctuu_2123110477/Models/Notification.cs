using System.ComponentModel.DataAnnotations;

namespace tranquoctuu_2123110477.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public bool IsSent { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // CÁC THUỘC TÍNH CÒN THIẾU GÂY LỖI:
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; } // Thêm dòng này để hết lỗi 'DeletedBy'

        // Quan hệ với bảng Customer
        public virtual Customer? Customer { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // Thêm thư viện này

namespace tranquoctuu_2123110477.Models;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Channel { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    [JsonIgnore] // Giấu Customer để không phải nhập JSON phức tạp
    public virtual Customer? Customer { get; set; } // Thêm ? để tránh lỗi Required

    public virtual ICollection<OrderItem>? OrderItems { get; set; } // Thêm ? để linh hoạt khi tạo đơn
}
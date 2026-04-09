using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // Thêm thư viện này

namespace tranquoctuu_2123110477.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    [JsonIgnore] 
    public virtual Order? Order { get; set; } // Thêm ? để sửa lỗi 400
}
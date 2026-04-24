namespace tranquoctuu_2123110477.Models;
using System.Text.Json.Serialization;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Channel { get; set; }
    public string? Status { get; set; }

    // 👇 THÊM 2 DÒNG NÀY VÀO 👇
    public string? Address { get; set; }
    public string? Phone { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    [JsonIgnore]
    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderItem>? OrderItems { get; set; }
}
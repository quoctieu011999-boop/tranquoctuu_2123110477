using System.ComponentModel.DataAnnotations;
namespace tranquoctuu_2123110477.Models;

namespace tranquoctuu_2123110477.Models
{
    public int Id { get; set; }
    public int OrderId { get; set; }

    public decimal Amount { get; set; }
    public string Method { get; set; } 
    public string Status { get; set; } 

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Order Order { get; set; }
}


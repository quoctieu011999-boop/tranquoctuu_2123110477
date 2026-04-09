using System.ComponentModel.DataAnnotations;
namespace tranquoctuu_2123110477.Models;

namespace tranquoctuu_2123110477.Models
{
    public int Id { get; set; }
    public int CustomerId { get; set; }

    public string Title { get; set; }
    public string Message { get; set; }
    public string Channel { get; set; }
    public bool IsSent { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Customer Customer { get; set; }
}
 

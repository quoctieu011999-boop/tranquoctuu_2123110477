using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tranquoctuu_2123110477.Models
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int TotalPoints { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Customer Customer { get; set; }
}

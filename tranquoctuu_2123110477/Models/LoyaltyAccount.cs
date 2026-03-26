using System.ComponentModel.DataAnnotations;

namespace tranquoctuu_2123110477.Models;
public class LoyaltyAccount
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int TotalPoints { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Customer Customer { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace ConnectDB.Models;

public class LoyaltyAccount
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int TotalPoints { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Customer Customer { get; set; }
}

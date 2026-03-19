using System.ComponentModel.DataAnnotations;
namespace ConnectDB.Models;
public class LoyaltyRule
{
    public int Id { get; set; }

    public string RuleName { get; set; }
    public int PointsPerAmount { get; set; }
    public int MinPointsToRedeem { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

using System.ComponentModel.DataAnnotations;
namespace ConnectDB.Models;

     public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }

    public decimal Amount { get; set; }
    public string Method { get; set; } 
    public string Status { get; set; } 

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Order Order { get; set; }
}


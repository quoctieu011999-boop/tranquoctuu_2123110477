using System.ComponentModel.DataAnnotations;
namespace ConnectDB.Models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public string Level { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<Order> Orders { get; set; }
}

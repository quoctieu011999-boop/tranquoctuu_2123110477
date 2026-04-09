using System.ComponentModel.DataAnnotations;

namespace tranquoctuu_2123110477.Models;

public class Customer
{
    [Key]
    public int Id { get; set; }

    [Required] 
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public string Level { get; set; }

   
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; } 

    public DateTime? UpdatedAt { get; set; } 
    public string? UpdatedBy { get; set; } 

    public DateTime? DeletedAt { get; set; } 
    public string? DeletedBy { get; set; } 
    // --------------------------------------

    public ICollection<Order>? Orders { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; 

namespace tranquoctuu_2123110477.Models;

public class CustomerInteraction
{
    public int Id { get; set; }
    public int CustomerId { get; set; }

    public string? InteractionType { get; set; } 
    public string? Content { get; set; }         
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonIgnore] 
    public virtual Customer? Customer { get; set; } 
}
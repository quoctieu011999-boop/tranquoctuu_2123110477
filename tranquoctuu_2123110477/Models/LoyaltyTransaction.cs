using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace tranquoctuu_2123110477.Models
{
    public class LoyaltyTransaction : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public int Points { get; set; } 

        [Required]
        [StringLength(20)]
        public string Type { get; set; } 

        public string? Description { get; set; }

       
        public string? ReferenceId { get; set; } 

        [Required]
        public string Hash { get; set; }

        public string? PreviousHash { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

       
        public string CalculateHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                
                string rawData = $"{CustomerId}|{Points}|{Type}|{ReferenceId}|{PreviousHash}|{CreatedAt}";
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
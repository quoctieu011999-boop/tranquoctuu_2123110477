using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace tranquoctuu_2123110477.Models
{
    public class LoyaltyTransaction : BaseEntity
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public int Points { get; set; }

        public string Type { get; set; } 
        public string Description { get; set; }

        
        public string Hash { get; set; }
        public string PreviousHash { get; set; }

        public bool IsDeleted { get; set; } = false;

 
        public Customer? Customer { get; set; }

     
        public string CalculateHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string rawData = $"{CustomerId}-{Points}-{Type}-{Description}-{PreviousHash}-{CreatedAt}";
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
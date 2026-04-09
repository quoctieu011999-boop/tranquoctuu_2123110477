using System.Security.Cryptography;
using System.Text;

namespace tranquoctuu_2123110477.Models
{
    public class LoyaltyTransaction
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        // Các thuộc tính phục vụ bảo mật/blockchain
        public string Hash { get; set; } = string.Empty;
        public string PreviousHash { get; set; } = "0";

       
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; } 
        public string? DeletedBy { get; set; }   

        
        public string CalculateHash()
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                string rawData = $"{Id}{CustomerId}{PreviousHash}{CreatedAt}";
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
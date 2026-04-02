using System.ComponentModel.DataAnnotations;

namespace tranquoctuu_2123110477.Models
{
    public class LoyaltyRule : BaseEntity
    {
        public int Id { get; set; }

        public string RuleName { get; set; }   
        public string Description { get; set; }

        public int Points { get; set; }       
        public string ActionType { get; set; } 

        public bool IsDeleted { get; set; } = false;
    }
}
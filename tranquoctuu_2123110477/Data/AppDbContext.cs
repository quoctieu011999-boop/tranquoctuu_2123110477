using Microsoft.EntityFrameworkCore;
using ConnectDB.Models;

namespace ConnectDB.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

      
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerChannel> CustomerChannels { get; set; }
        public DbSet<CustomerInteraction> CustomerInteractions { get; set; }

      
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

   
        public DbSet<LoyaltyAccount> LoyaltyAccounts { get; set; }
        public DbSet<LoyaltyTransaction> LoyaltyTransactions { get; set; }

    
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Redemption> Redemptions { get; set; }

    
        public DbSet<Payment> Payments { get; set; }

     
        public DbSet<Notification> Notifications { get; set; }

      
        public DbSet<LoyaltyRule> LoyaltyRules { get; set; }

      
    }
}
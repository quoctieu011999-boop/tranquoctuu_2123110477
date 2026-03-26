using ConnectDB.Models;
using Microsoft.EntityFrameworkCore;
using tranquoctuu_2123110477.Models; 

namespace tranquoctuu_2123110477.Data 
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Danh sách các bảng
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
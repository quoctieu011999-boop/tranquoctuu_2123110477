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

        // 1. Quản lý Tài khoản Hệ thống
        public DbSet<Users> Users { get; set; } // THÊM DÒNG NÀY ĐỂ HẾT LỖI Ở UsersController

        // 2. Quản lý Khách hàng & Tương tác
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerChannel> CustomerChannels { get; set; }
        public DbSet<CustomerInteraction> CustomerInteractions { get; set; }

        // 3. Quản lý Sản phẩm & Đơn hàng
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        // 4. Quản lý Loyalty & Reward
        public DbSet<LoyaltyAccount> LoyaltyAccounts { get; set; }
        public DbSet<LoyaltyTransaction> LoyaltyTransactions { get; set; }
        public DbSet<LoyaltyRule> LoyaltyRules { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Redemption> Redemptions { get; set; }

        // 5. Thanh toán & Thông báo
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        // Cấu hình Fluent API (Nếu cần định nghĩa quan hệ phức tạp)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ví dụ: Đảm bảo Username trong bảng User là duy nhất
            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
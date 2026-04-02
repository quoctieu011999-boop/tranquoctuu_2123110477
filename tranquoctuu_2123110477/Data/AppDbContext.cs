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

        // ==================== DbSets ====================
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; } // Đã cập nhật thành DbSet
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerChannel> CustomerChannels { get; set; }
        public DbSet<CustomerInteraction> CustomerInteractions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<LoyaltyAccount> LoyaltyAccounts { get; set; }
        public DbSet<LoyaltyTransaction> LoyaltyTransactions { get; set; }
        public DbSet<LoyaltyRule> LoyaltyRules { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Redemption> Redemptions { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        // ==================== SaveChanges Override ====================
        public override int SaveChanges()
        {
            ApplyAudit();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            ApplyAudit();
            return await base.SaveChangesAsync(cancellationToken);
        }

        // ==================== Audit & Soft Delete ====================
        private void ApplyAudit()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var username = "admin"; // Trong thực tế sẽ lấy từ User.Identity.Name

            foreach (var entry in entries)
            {
                var entity = entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.Now;
                    entity.CreatedBy = username;
                    entity.IsDeleted = false;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.Now;
                    entity.UpdatedBy = username;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    // Chặn xóa vật lý, chuyển thành xóa mềm (Soft Delete)
                    entry.State = EntityState.Modified;
                    entity.IsDeleted = true;
                    entity.DeletedAt = DateTime.Now;
                    entity.DeletedBy = username;
                    entity.UpdatedAt = DateTime.Now;
                }
            }
        }

        // ==================== Model Configuration ====================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --------- Soft Delete Filter (Tự động lọc dữ liệu chưa xóa) ---------
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(AppDbContext)
                        .GetMethod(nameof(SetSoftDeleteFilter),
                            System.Reflection.BindingFlags.NonPublic |
                            System.Reflection.BindingFlags.Static)
                        ?.MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(null, new object[] { modelBuilder });
                }
            }

            // --------- Decimal Precision ---------
            modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Order>().Property(o => o.TotalAmount).HasPrecision(18, 2);
            modelBuilder.Entity<OrderItem>().Property(oi => oi.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Payment>().Property(p => p.Amount).HasPrecision(18, 2);

            // --------- Relationships ---------

            // Quan hệ Category - Product (1-N)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Redemption>()
                .HasOne(r => r.Customer)
                .WithMany()
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Redemption>()
                .HasOne(r => r.Reward)
                .WithMany(rw => rw.Redemptions)
                .HasForeignKey(r => r.RewardId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        // ==================== Global Soft Delete Filter ====================
        private static void SetSoftDeleteFilter<TEntity>(ModelBuilder builder)
            where TEntity : BaseEntity
        {
            builder.Entity<TEntity>()
                   .HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
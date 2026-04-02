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

     
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerChannel> CustomerChannels { get; set; }
        public DbSet<CustomerInteraction> CustomerInteractions { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<LoyaltyAccount> LoyaltyAccounts { get; set; }
        public DbSet<LoyaltyTransaction> LoyaltyTransactions { get; set; }
        public DbSet<LoyaltyRule> LoyaltyRules { get; set; }

        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Redemption> Redemptions { get; set; }

        public DbSet<Notification> Notifications { get; set; }

      
        public override int SaveChanges()
        {
            ApplyAudit();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAudit();
            return await base.SaveChangesAsync(cancellationToken);
        }

     
        private void ApplyAudit()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            var username = "admin";

            foreach (var entry in entries)
            {
                var entity = entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.Now;
                    entity.CreatedBy = username;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.Now;
                    entity.UpdatedBy = username;
                }
                else if (entry.State == EntityState.Deleted)
                {
                  
                    entry.State = EntityState.Modified;

                    entity.DeletedAt = DateTime.Now;
                    entity.DeletedBy = username;

                    entity.UpdatedAt = DateTime.Now; 
                }
            }
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(AppDbContext)
                        .GetMethod(nameof(SetSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                        .MakeGenericMethod(entityType.ClrType);

                    method.Invoke(null, new object[] { modelBuilder });
                }
            }

           

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId);

            modelBuilder.Entity<Redemption>()
                .HasOne(r => r.Customer)
                .WithMany()
                .HasForeignKey(r => r.CustomerId);

            modelBuilder.Entity<Redemption>()
                .HasOne(r => r.Reward)
                .WithMany(rw => rw.Redemptions)
                .HasForeignKey(r => r.RewardId);
        }

       
        private static void SetSoftDeleteFilter<TEntity>(ModelBuilder builder)
            where TEntity : BaseEntity
        {
            builder.Entity<TEntity>().HasQueryFilter(x => x.DeletedAt == null);
        }
    }
}
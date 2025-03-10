using OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Infrastructure.Persistence
{
    internal class OrderServiceDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public OrderServiceDbContext(DbContextOptions<OrderServiceDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>().HasKey(o => o.Id);
            modelBuilder.Entity<Order>().Property(o => o.ProductId).IsRequired();
            modelBuilder.Entity<Order>().Property(o => o.Quantity).IsRequired();
        }
    }
}

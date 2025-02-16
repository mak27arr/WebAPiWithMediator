using Inventory.Domain.Entities;
using Inventory.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence
{
    internal class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
            : base(options)
        { }

        public DbSet<ProductInventory> ProductInventories { get; set; }

        public DbSet<InventoryEvent> InventoryEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
            modelBuilder.Entity<ProductInventory>()
                .HasKey(p => p.ProductId);

            modelBuilder.Entity<InventoryEvent>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<InventoryEvent>()
                .Property(e => e.ProductId)
                .IsRequired();

            modelBuilder.Entity<InventoryEvent>()
                .Property(e => e.Quantity)
                .IsRequired();

            modelBuilder.Entity<InventoryEvent>()
                .Property(e => e.Action)
                .IsRequired();

            modelBuilder.Entity<InventoryEvent>()
                .Property(e => e.Timestamp)
                .IsRequired();
        }
    }
}

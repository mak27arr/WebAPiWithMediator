using Inventory.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence
{
    public class EventStoreDbContext : DbContext
    {
        public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options)
            : base(options)
        { }

        public DbSet<InventoryEvent> InventoryEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

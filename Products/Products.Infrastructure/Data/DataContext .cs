using Microsoft.EntityFrameworkCore;
using Products.Infrastructure.Models;

namespace Products.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<ProductPriceHistory> ProductPriceHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Currency)
                .WithMany()
                .HasForeignKey(p => p.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Currency>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Currency>()
                .HasIndex(c => c.Code)
                .IsUnique();

            modelBuilder.Entity<ProductPriceHistory>()
                .HasKey(pph => pph.Id);

            modelBuilder.Entity<ProductPriceHistory>()
                .Property(pph => pph.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ProductPriceHistory>()
                .HasOne(pph => pph.Product)
                .WithMany()
                .HasForeignKey(pph => pph.ProductId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<ProductPriceHistory>()
                .HasOne(pph => pph.Currency)
                .WithMany()
                .HasForeignKey(pph => pph.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

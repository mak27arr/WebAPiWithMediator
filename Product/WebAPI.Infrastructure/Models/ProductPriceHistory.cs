namespace WebAPI.Infrastructure.Models
{
    public class ProductPriceHistory
    {
        public long Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public double Price { get; set; }

        public DateTime ChangedAtUtc { get; set; }

        public int CurrencyId { get; set; }

        public Currency Currency { get; set; }
    }
}

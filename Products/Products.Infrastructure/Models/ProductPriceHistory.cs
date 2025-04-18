namespace Products.Infrastructure.Models
{
    public class ProductPriceHistory
    {
        public long Id { get; set; }

        public int ProductId { get; set; }

        public required Product Product { get; set; }

        public double Price { get; set; }

        public DateTime ChangedAtUtc { get; set; }

        public int CurrencyId { get; set; }

        public required Currency Currency { get; set; }
    }
}

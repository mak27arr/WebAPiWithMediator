namespace Products.Core.DTOs
{
    public class ProductPriceHistoryDTO
    {
        public long Id { get; set; }

        public int ProductId { get; set; }

        public ProductDTO Product { get; set; }

        public double Price { get; set; }

        public DateTime ChangedAtUtc { get; set; }

        public int CurrencyId { get; set; }

        public CurrencyDTO Currency { get; set; }
    }
}

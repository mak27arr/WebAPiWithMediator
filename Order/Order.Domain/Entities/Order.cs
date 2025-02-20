namespace Order.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public int? ProductId { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

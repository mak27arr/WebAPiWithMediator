namespace Inventory.Domain.Entities
{
    public class ProductInventory
    {
        public int? ProductId { get; init; }  
  
        public int? Quantity { get; private set; }

        public void Reserve(int amount)
        {
            Remove(amount);
        }

        public void Add(int amount)
        {
            if (amount < 0)
                throw new InvalidOperationException($"{amount} below zero");

            Quantity += amount;
        }

        public void Remove(int amount)
        {
            if (amount > Quantity)
                throw new InvalidOperationException("Not enough stock");

            Quantity -= amount;
        }
    }
}

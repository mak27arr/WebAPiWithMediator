using Inventory.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Infrastructure.Entity
{
    public class ProductInventoryEntity
    {
        [Key]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        internal void UpdateFromProduct(ProductInventory productInventory)
        {
            Quantity = productInventory.Quantity ?? 0;
        }
    }
}

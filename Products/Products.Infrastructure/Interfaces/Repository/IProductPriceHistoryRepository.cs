using Products.Infrastructure.Models;

namespace Products.Infrastructure.Interfaces.Repository
{
    public interface IProductPriceHistoryRepository
    {
        Task AddAsync(ProductPriceHistory history);
    }
}

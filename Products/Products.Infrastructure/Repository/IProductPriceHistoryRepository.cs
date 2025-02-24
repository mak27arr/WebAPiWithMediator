using Products.Infrastructure.Models;

namespace Products.Infrastructure.Repository
{
    public interface IProductPriceHistoryRepository
    {
        Task AddAsync(ProductPriceHistory history);
    }
}

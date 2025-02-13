using WebAPI.Infrastructure.Models;

namespace WebAPI.Core.Repository
{
    public interface IProductPriceHistoryRepository
    {
        Task AddAsync(ProductPriceHistory history);
    }
}

using WebAPI.Core.Models;

namespace WebAPI.Core.Repository
{
    public interface IProductPriceHistoryRepository
    {
        Task AddAsync(ProductPriceHistory history);
    }
}

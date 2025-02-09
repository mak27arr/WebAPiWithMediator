using WebAPI.Core.Models;

namespace WebAPI.Core.Repository
{
    public interface ICurrencyRepository
    {
        Task<int> AddAsync(Currency currency);
        Task<Currency> GetByIdAsync(int id);
        Task<Currency> GetByCodeAsync(string code);
        Task<List<Currency>> GetAllAsync();
    }
}

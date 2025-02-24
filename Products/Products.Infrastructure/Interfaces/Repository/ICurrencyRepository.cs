using Products.Infrastructure.Models;

namespace Products.Infrastructure.Interfaces.Repository
{
    public interface ICurrencyRepository
    {
        Task<int> AddAsync(Currency currency);
        Task<Currency> GetByIdAsync(int id);
        Task<Currency> GetByCodeAsync(string code);
        Task<List<Currency>> GetAllAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using WebAPI.Core.Repository;
using WebAPI.Infrastructure.Models;

namespace WebAPI.Infrastructure.Data
{
    internal class CurrencyRepository : ICurrencyRepository
    {
        private readonly DataContext _context;

        public CurrencyRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Currency currency)
        {
            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync();
            return currency.Id;
        }

        public async Task<Currency> GetByIdAsync(int id) => await _context.Currencies.FindAsync(id);

        public async Task<Currency> GetByCodeAsync(string code) => await _context.Currencies.FirstOrDefaultAsync(c => c.Code == code);

        public async Task<List<Currency>> GetAllAsync() => await _context.Currencies.ToListAsync();
    }
}

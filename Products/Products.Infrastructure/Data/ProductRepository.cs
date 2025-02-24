using Products.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Products.Infrastructure.Models;

namespace Products.Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetPaginatedProductsAsync(int pageIndex, int pageSize)
        {
            var items = await _context.Products.AsQueryable()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return items;
        }

        public async Task<int> GetPageCountAsync(int pageSize)
        {
            return (int) Math.Ceiling((double)await _context.Products.AsQueryable().CountAsync() / pageSize);
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Include(x => x.Currency).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}

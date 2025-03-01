using Products.Infrastructure.Models;

namespace Products.Infrastructure.Interfaces.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetPaginatedProductsAsync(int pageIndex, int pageSize);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<int> GetPageCountAsync(int pageSize);
    }
}

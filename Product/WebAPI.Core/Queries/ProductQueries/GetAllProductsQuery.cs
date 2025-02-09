using MediatR;
using WebAPI.Core.Models.Products;

namespace WebAPI.Core.Queries.ProductQueries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>> { }
}

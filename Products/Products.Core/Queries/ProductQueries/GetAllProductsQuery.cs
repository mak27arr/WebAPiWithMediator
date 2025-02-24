using MediatR;
using Products.Core.DTOs;

namespace Products.Core.Queries.ProductQueries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductDTO>> { }
}

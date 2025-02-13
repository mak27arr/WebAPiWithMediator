using MediatR;
using WebAPI.Core.DTOs;

namespace WebAPI.Core.Queries.ProductQueries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductDTO>> { }
}

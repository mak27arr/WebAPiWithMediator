using MediatR;
using WebAPI.Core.Models;

namespace WebAPI.Core.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>> { }
}

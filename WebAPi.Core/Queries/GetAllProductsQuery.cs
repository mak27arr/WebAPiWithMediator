using MediatR;
using WebAPI.Core.Models;

namespace WebAPi.Core.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>> { }
}

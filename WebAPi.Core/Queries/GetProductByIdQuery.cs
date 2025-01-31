using MediatR;
using WebAPI.Core.Models;

namespace WebAPi.Core.Queries
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; set; }
    }
}

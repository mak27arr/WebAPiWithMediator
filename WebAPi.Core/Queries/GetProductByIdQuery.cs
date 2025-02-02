using MediatR;
using WebAPI.Core.Models;

namespace WebAPI.Core.Queries
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; set; }
    }
}

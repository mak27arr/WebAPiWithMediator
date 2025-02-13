using MediatR;
using WebAPI.Core.DTOs;

namespace WebAPI.Core.Queries.ProductQueries
{
    public class GetProductByIdQuery : IRequest<ProductDTO>
    {
        public int Id { get; set; }
    }
}

using MediatR;
using Products.Core.DTOs;

namespace Products.Core.Queries.ProductQueries
{
    public class GetProductByIdQuery : IRequest<ProductDTO>
    {
        public int Id { get; set; }
    }
}

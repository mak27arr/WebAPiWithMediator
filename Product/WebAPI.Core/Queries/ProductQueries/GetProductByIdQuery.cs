using MediatR;
using WebAPI.Core.Models.Products;

namespace WebAPI.Core.Queries.ProductQueries
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; set; }
    }
}

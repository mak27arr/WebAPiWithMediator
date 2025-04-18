using MediatR;
using Products.Core.DTOs;

namespace Products.Core.Commands.Products
{
    public class CreateProductCommand : IRequest<ProductDTO>
    {
        public required string Name { get; set; }
    }
}

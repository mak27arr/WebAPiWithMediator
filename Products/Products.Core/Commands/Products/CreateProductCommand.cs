using MediatR;
using Products.Core.DTOs;

namespace Products.Core.Commands.Products
{
    public class CreateProductCommand : IRequest<ProductDTO>
    {
        public string Name { get; set; }
    }
}

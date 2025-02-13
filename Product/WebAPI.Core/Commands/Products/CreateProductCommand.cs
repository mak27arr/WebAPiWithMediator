using MediatR;
using WebAPI.Core.DTOs;

namespace WebAPI.Core.Commands.Products
{
    public class CreateProductCommand : IRequest<ProductDTO>
    {
        public string Name { get; set; }
    }
}

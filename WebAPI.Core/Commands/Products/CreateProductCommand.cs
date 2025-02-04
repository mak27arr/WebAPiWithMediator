using MediatR;
using WebAPI.Core.Models.Products;

namespace WebAPI.Core.Commands.Products
{
    public class CreateProductCommand : IRequest<Product>
    {
        public string Name { get; set; }
    }
}

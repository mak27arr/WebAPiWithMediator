using MediatR;
using WebAPI.Core.Models;

namespace WebAPI.Core.Commands
{
    public class CreateProductCommand : IRequest<Product>
    {
        public string Name { get; set; }
    }
}

using MediatR;
using WebAPI.Core.Models;

namespace WebAPi.Core.Commands
{
    public class UpdateProductCommand : IRequest<Product>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}

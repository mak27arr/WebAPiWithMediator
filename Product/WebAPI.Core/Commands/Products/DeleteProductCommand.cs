using MediatR;

namespace WebAPI.Core.Commands.Products
{
    public class DeleteProductCommand : IRequest
    {
        public int Id { get; set; }
    }
}

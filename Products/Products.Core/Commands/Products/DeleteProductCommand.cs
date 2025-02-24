using MediatR;

namespace Products.Core.Commands.Products
{
    public class DeleteProductCommand : IRequest
    {
        public int Id { get; set; }
    }
}

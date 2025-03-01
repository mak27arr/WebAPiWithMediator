using MediatR;
using OneOf.Types;
using OneOf;

namespace Products.Core.Commands.Products
{
    public class DeleteProductCommand : IRequest<OneOf<Success, NotFound>>
    {
        public int Id { get; set; }
    }
}

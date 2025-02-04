using MediatR;
using OneOf.Types;
using OneOf;

namespace WebAPI.Core.Commands.Products
{
    public class UpdateProductCommand : IRequest<OneOf<Success, NotFound>>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}

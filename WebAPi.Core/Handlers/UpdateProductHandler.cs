using MediatR;
using OneOf;
using OneOf.Types;
using WebAPI.Core.Commands;
using WebAPI.Core.Models;
using WebAPI.Core.Repository;

namespace WebAPI.Core.Handlers
{
    internal class UpdateProductHandler : IRequestHandler<UpdateProductCommand, OneOf<Success, NotFound>>
    {
        private readonly IProductRepository _repository;

        public UpdateProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<OneOf<Success, NotFound>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetProductByIdAsync(request.Id);
            if (product == null)
                return new NotFound();

            await _repository.UpdateProductAsync(product);

            return new Success();
        }
    }
}

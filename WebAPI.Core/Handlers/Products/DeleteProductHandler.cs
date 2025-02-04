using MediatR;
using WebAPI.Core.Commands.Products;
using WebAPI.Core.Repository;

namespace WebAPI.Core.Handlers.Products
{
    internal class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _repository;

        public DeleteProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteProductAsync(request.Id);
        }
    }
}

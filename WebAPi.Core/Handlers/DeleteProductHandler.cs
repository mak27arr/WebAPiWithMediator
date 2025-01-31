using MediatR;
using WebAPi.Core.Commands;
using WebAPi.Core.Repository;

namespace WebAPI.Core.Handlers
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

using MediatR;
using WebAPI.Core.Models;
using WebAPi.Core.Queries;
using WebAPi.Core.Repository;

namespace WebAPi.Core.Handlers
{
    internal class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IProductRepository _repository;

        public GetProductByIdHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetProductByIdAsync(request.Id);
        }
    }
}

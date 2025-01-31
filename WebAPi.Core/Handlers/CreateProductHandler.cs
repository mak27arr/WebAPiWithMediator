using MediatR;
using WebAPi.Core.Repository;
using WebAPI.Core.Commands;
using WebAPI.Core.Models;

namespace WebAPI.Core.Handlers
{
    internal class CreateProductHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly IProductRepository _repository;

        public CreateProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name
            };

            return await _repository.AddProductAsync(product);
        }
    }
}

using AutoMapper;
using MediatR;
using OneOf;
using OneOf.Types;
using Products.Core.Commands.Products;
using Products.Infrastructure.Repository;

namespace Products.Core.Handlers.Products
{
    internal class UpdateProductHandler : IRequestHandler<UpdateProductCommand, OneOf<Success, NotFound>>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _repository;

        public UpdateProductHandler(IMapper mapper, IProductRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<OneOf<Success, NotFound>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetProductByIdAsync(request.Id);

            if (product == null)
                return new NotFound();

            _mapper.Map(request, product);
            await _repository.UpdateProductAsync(product);

            return new Success();
        }
    }
}

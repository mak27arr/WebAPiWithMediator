using MediatR;
using WebAPI.Core.Commands.Products.PriceCommands;
using WebAPI.Core.Repository;

namespace WebAPI.Core.Handlers.Products.PriceHandlers
{
    public class ProductPriceCommandHandler : IRequestHandler<AddProductPriceCommand, long>
    {
        private readonly IProductPriceHistoryRepository _productPriceRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICurrencyRepository _currencyRepository;

        public ProductPriceCommandHandler(
            IProductPriceHistoryRepository productPriceRepository,
            IProductRepository productRepository,
            ICurrencyRepository currencyRepository)
        {
            _productPriceRepository = productPriceRepository;
            _productRepository = productRepository;
            _currencyRepository = currencyRepository;
        }

        public async Task<long> Handle(AddProductPriceCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByIdAsync(request.ProductId);
            if (product == null)
                throw new ArgumentException("Product not found.");

            var currency = await _currencyRepository.GetByIdAsync(request.CurrencyId);
            if (currency == null)
                throw new ArgumentException("Currency not found.");

            var price = new Models.ProductPriceHistory
            {
                ProductId = request.ProductId,
                Price = request.Price,
                CurrencyId = request.CurrencyId,
                ChangedAtUtc = DateTime.UtcNow
            };

            await _productPriceRepository.AddAsync(price);

            product.CurrentPrice = request.Price;
            product.Currency = currency; 

            await _productRepository.UpdateProductAsync(product);

            return price.Id;
        }
    }
}

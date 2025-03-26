using Inventory.Application.Interface;

namespace Inventory.Application.Services.Grpc
{
    internal class ProductGrpcService : IProductGrpcService
    {
        private readonly ProductService.Grpc.ProductService.ProductServiceClient _client;

        public ProductGrpcService(ProductService.Grpc.ProductService.ProductServiceClient client)
        {
            _client = client;
        }

        public async Task<bool> ProductExistsAsync(int productId)
        {
            var request = new ProductService.Grpc.ProductRequest { ProductId = productId };

            var response = await _client.GetProductByIdAsync(request);

            return response?.ProductId != null;
        }
    }
}

namespace Inventory.Application.Interface
{
    internal interface IProductGrpcService
    {
        Task<bool> ProductExistsAsync(int productId);
    }
}

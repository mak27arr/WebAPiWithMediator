namespace Products.Common.Protos.Product
{
    public interface IProductGrpcService
    {
        Task<bool> ProductExistsAsync(int productId);
    }
}

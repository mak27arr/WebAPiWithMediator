using MediatR;

namespace Products.Core.Commands.Products.PriceCommands
{
    public class AddProductPriceCommand : IRequest<long>
    {
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int CurrencyId { get; set; }
    }
}

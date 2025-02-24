using MediatR;
using Products.Core.DTOs;

namespace Products.Core.Queries.CurrenciesQueries
{
    public class GetAllCurrenciesQuery : IRequest<List<CurrencyDTO>>
    {
    }
}

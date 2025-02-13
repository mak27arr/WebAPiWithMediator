using MediatR;
using WebAPI.Core.DTOs;

namespace WebAPI.Core.Queries.CurrenciesQueries
{
    public class GetAllCurrenciesQuery : IRequest<List<CurrencyDTO>>
    {
    }
}

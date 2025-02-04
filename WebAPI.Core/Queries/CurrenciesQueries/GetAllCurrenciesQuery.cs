using MediatR;
using WebAPI.Core.Models;

namespace WebAPI.Core.Queries.CurrenciesQueries
{
    public class GetAllCurrenciesQuery : IRequest<List<Currency>>
    {
    }
}

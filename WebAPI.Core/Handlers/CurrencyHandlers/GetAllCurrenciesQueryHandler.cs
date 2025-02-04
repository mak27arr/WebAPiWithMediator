using MediatR;
using WebAPI.Core.Models;
using WebAPI.Core.Queries.CurrenciesQueries;
using WebAPI.Core.Repository;

namespace WebAPI.Core.Handlers.CurrencyHandlers
{
    public class GetAllCurrenciesQueryHandler : IRequestHandler<GetAllCurrenciesQuery, List<Currency>>
    {
        private readonly ICurrencyRepository _currencyRepository;

        public GetAllCurrenciesQueryHandler(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<List<Currency>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
        {
            return await _currencyRepository.GetAllAsync();
        }
    }
}

using AutoMapper;
using MediatR;
using Products.Core.DTOs;
using Products.Core.Queries.CurrenciesQueries;
using Products.Infrastructure.Interfaces.Repository;

namespace Products.Core.Handlers.CurrencyHandlers
{
    public class GetAllCurrenciesQueryHandler : IRequestHandler<GetAllCurrenciesQuery, List<CurrencyDTO>>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        public GetAllCurrenciesQueryHandler(ICurrencyRepository currencyRepository, IMapper mapper)
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }

        public async Task<List<CurrencyDTO>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var currencies = await _currencyRepository.GetAllAsync();
            return _mapper.Map<List<CurrencyDTO>>(currencies);
        }
    }
}

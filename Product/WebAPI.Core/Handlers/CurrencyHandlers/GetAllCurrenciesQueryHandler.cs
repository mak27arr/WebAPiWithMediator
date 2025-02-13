using AutoMapper;
using MediatR;
using WebAPI.Core.DTOs;
using WebAPI.Core.Queries.CurrenciesQueries;
using WebAPI.Core.Repository;

namespace WebAPI.Core.Handlers.CurrencyHandlers
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

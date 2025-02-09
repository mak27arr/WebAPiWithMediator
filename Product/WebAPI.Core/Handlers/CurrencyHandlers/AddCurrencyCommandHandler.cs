using MediatR;
using WebAPI.Core.Repository;
using WebAPI.Core.Models;
using WebAPI.Core.Commands.CurrencyCommands;

namespace WebAPI.Core.Handlers.CurrencyHandlers
{
    internal class AddCurrencyCommandHandler : IRequestHandler<AddCurrencyCommand, int>
    {
        private readonly ICurrencyRepository _currencyRepository;

        public AddCurrencyCommandHandler(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<int> Handle(AddCurrencyCommand request, CancellationToken cancellationToken)
        {
            var existingCurrency = await _currencyRepository.GetByCodeAsync(request.Code);

            if (existingCurrency != null)
                throw new ArgumentException("Currency with this code already exists");

            var currency = new Currency
            {
                Code = request.Code,
                Name = request.Name
            };

            return await _currencyRepository.AddAsync(currency);
        }
    }
}

using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Core.Commands.CurrencyCommands;
using Products.Core.Queries.CurrenciesQueries;

namespace Products.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/currencies")]
    [ApiVersion("1.0")]
    [Authorize]
    public class CurrencyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CurrencyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllCurrencies()
        {
            var currencies = await _mediator.Send(new GetAllCurrenciesQuery());

            return Ok(currencies);
        }

        [HttpPost]
        public async Task<IActionResult> AddCurrency([FromBody] AddCurrencyCommand command)
        {
            var currencyId = await _mediator.Send(command);

            return Created("", new { id = currencyId });
        }
    }
}

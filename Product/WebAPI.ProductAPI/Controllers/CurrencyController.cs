using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Core.Commands.CurrencyCommands;
using WebAPI.Core.Queries.CurrenciesQueries;

namespace WebAPI.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class CurrencyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CurrencyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCurrencies()
        {
            var currencies = await _mediator.Send(new GetAllCurrenciesQuery());

            return Ok(currencies);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCurrency([FromBody] AddCurrencyCommand command)
        {
            var currencyId = await _mediator.Send(command);

            return Created("", new { id = currencyId });
        }
    }
}

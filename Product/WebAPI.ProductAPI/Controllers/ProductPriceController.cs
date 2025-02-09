using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Core.Commands.Products.PriceCommands;

namespace WebAPI.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class ProductPriceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductPriceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("{productId}/price")]
        public async Task<IActionResult> AddProductPrice(int productId, [FromBody] AddProductPriceCommand command)
        {
            command.ProductId = productId;
            var priceId = await _mediator.Send(command);

            return Created("", new { productId, id = priceId });
        }
    }

}

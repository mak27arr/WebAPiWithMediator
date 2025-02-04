using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Core.Commands.Products.PriceCommands;

namespace WebAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductPriceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductPriceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{productId}/price")]
        public async Task<IActionResult> AddProductPrice(int productId, [FromBody] AddProductPriceCommand command)
        {
            command.ProductId = productId;
            var priceId = await _mediator.Send(command);

            return Created("", new { productId, id = priceId });
        }
    }

}

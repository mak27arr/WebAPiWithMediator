using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Features.Commands;

namespace OrderService.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var orderId = await _mediator.Send(command);
            return orderId != Guid.Empty ? Ok(new { OrderId = orderId }) : BadRequest();
        }
    }
}

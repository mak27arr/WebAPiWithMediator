using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Features.Commands;
using Products.Common.Auth.Role;

namespace OrderService.API.Controllers
{
    [Route("api/v{version:apiVersion}/orders")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize(Roles = $"{UserRoles.Manager}")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var orderId = await _mediator.Send(command);
            return orderId != Guid.Empty ? Ok(new { OrderId = orderId }) : BadRequest();
        }
    }
}

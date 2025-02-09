using JwtAuthManager.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var token = await _mediator.Send(command);

            if (string.IsNullOrEmpty(token))
                return Unauthorized();

            return Ok(new { Token = token });
        }
    }
}

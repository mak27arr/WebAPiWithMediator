using Microsoft.AspNetCore.Mvc;
using MediatR;
using UserService.Application.Feature.Queries;
using Microsoft.AspNetCore.Authorization;
using Products.Common.Auth.Role;
using UserService.Application.Feature.Command;
using UserService.Domain.Entities;
using Asp.Versioning;
namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize($"{UserRoles.Admin}")]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfileAsync(Guid userId)
        {
            var query = new GetUserByIdQuery(userId);
            var profile = await _mediator.Send(query);

            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserProfile>> GetOrCreateProfile()
        {
            var result = await _mediator.Send(new GetOrCreateUserProfileQuery());
            return Ok(result);
        }
    }

}

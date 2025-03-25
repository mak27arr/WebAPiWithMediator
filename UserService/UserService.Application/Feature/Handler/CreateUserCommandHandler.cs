using MediatR;
using UserService.Application.Feature.Command;
using UserService.Application.Interface.Services;

namespace UserService.Application.Feature.Handler
{
    internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
    {
        private readonly IUserContextService _userContextService;
        private readonly IUserProfileService _userProfileService;

        public CreateUserCommandHandler(IUserContextService userContextService, IUserProfileService userProfileService)
        {
            _userContextService = userContextService;
            _userProfileService = userProfileService;
        }

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetUserId();
            var email = _userContextService.GetEmail();
            var userName = _userContextService.GetUserName();

            var userProfile = await _userProfileService.CreateUserProfileAsync(userId, email, userName);

            return true;
        }
    }

}

using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Feature.Command;
using UserService.Application.Interface.Services;

namespace UserService.Application.Feature.Handler
{
    internal class GetOrCreateUserProfileHandler : IRequestHandler<GetOrCreateUserProfileQuery, UserProfileDto>
    {
        private readonly IUserContextService _userContextService;
        private readonly IUserProfileService _userProfileService;
        private readonly IMapper _mapper;

        public GetOrCreateUserProfileHandler(
        IUserContextService userContextService,
        IUserProfileService userProfileService, 
        IMapper mapper)
        {
            _userContextService = userContextService;
            _userProfileService = userProfileService;
            _mapper = mapper;
        }

        public async Task<UserProfileDto> Handle(GetOrCreateUserProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetUserId();
            var email = _userContextService.GetEmail();
            var userName = _userContextService.GetUserName();

            var userProfile = await _userProfileService.GetOrCreateUserProfileAsync(userId, email, userName);

            return _mapper.Map<UserProfileDto>(userProfile);
        }
    }
}

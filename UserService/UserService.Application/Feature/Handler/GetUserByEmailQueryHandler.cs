using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Feature.Queries;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.Application.Feature.Handler
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserProfileDto?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserByEmailQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserProfileDto?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            return _mapper.Map<UserProfileDto>(user);
        }
    }
}

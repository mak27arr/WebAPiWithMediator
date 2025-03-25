using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Feature.Queries;
using UserService.Domain.Repositories;

namespace UserService.Application.Feature.Handler
{
    internal class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserProfileDto?>
    {
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;

        public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserProfileDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user =  await _userRepository.GetByIdAsync(request.Id);

            return _mapper.Map<UserProfileDto>(user);
        }
    }
}

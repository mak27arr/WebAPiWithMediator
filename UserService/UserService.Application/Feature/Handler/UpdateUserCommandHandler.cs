using MediatR;
using UserService.Application.Feature.Command;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.Application.Feature.Handler
{
    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserProfile?>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserProfile?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
                throw new KeyNotFoundException($"User: {request.Id}");

            user.SetUserName(request.Name);
            await _userRepository.UpdateAsync(user);
            return user;
        }
    }
}

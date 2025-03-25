using MediatR;
using UserService.Application.Feature.Command;
using UserService.Domain.Repositories;

namespace UserService.Application.Feature.Handler
{
    internal class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _userRepository.DeleteAsync(request.Id);
            return true;
        }
    }
}

using MediatR;
using UserService.Domain.Entities;

namespace UserService.Application.Feature.Command
{
    public record CreateUserCommand(string Email, string Username) : IRequest<bool>;
}

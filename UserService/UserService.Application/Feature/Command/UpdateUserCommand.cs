using MediatR;
using UserService.Domain.Entities;

namespace UserService.Application.Feature.Command
{

    public record UpdateUserCommand(Guid Id, string Name) : IRequest<UserProfile?>;
}

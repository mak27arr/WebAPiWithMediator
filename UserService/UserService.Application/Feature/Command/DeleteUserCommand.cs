using MediatR;

namespace UserService.Application.Feature.Command
{
    public record DeleteUserCommand(Guid Id) : IRequest<bool>;
}

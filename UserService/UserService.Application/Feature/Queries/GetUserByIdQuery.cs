using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.Feature.Queries
{
    public record GetUserByIdQuery(Guid Id) : IRequest<UserProfileDto?>;
}

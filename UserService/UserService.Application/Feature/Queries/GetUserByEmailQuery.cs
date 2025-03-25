using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.Feature.Queries
{
    public record GetUserByEmailQuery(string Email) : IRequest<UserProfileDto?>;
}

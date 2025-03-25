using MediatR;
using UserService.Application.DTOs;
using UserService.Domain.Entities;

namespace UserService.Application.Feature.Command
{
    public record GetOrCreateUserProfileQuery : IRequest<UserProfileDto>;
}

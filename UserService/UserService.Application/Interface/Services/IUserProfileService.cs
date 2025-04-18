using UserService.Domain.Entities;

namespace UserService.Application.Interface.Services
{
    public interface IUserProfileService
    {
        Task<UserProfile> CreateUserProfileAsync(string userId, string email, string userName);

        Task<UserProfile> GetOrCreateUserProfileAsync(string userId, string email, string userName);
    }
}

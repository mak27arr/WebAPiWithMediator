using UserService.Application.Interface.Services;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.Application.Services
{
    internal class UserProfileService : IUserProfileService
    {
        private readonly IUserRepository _repository;

        public UserProfileService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserProfile> CreateUserProfileAsync(string userId, string email, string userName)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID not found.");

            var userProfile = new UserProfile(userId, email, userName);
            await _repository.CreateAsync(userProfile);
            return userProfile;
        }

        public async Task<UserProfile> GetOrCreateUserProfileAsync(string userId, string email, string userName)
        {
            var userProfile = await _repository.GetByUserIdAsync(userId);

            if (userProfile == null)
                userProfile = await CreateUserProfileAsync(userId, email, userName);

            return userProfile;
        }
    }
}

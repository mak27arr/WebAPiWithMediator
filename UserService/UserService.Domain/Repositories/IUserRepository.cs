using UserService.Domain.Entities;

namespace UserService.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<UserProfile?> GetByEmailAsync(string email);
        Task<UserProfile?> GetByIdAsync(Guid id);
        Task CreateAsync(UserProfile user);
        Task UpdateAsync(UserProfile user);
        Task DeleteAsync(Guid id);
        Task<UserProfile> GetByUserIdAsync(string userId);
    }
}

namespace UserService.Domain.Entities
{
    public class UserProfile
    {
        public Guid Id { get; private set; }

        public string UserId { get; private set; }

        public string Email { get; private set; }

        public string UserName { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public UserProfile(string userId, string email, string userName)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Email = email;
            UserName = userName;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetUserName(string name)
        {
            UserName = name;
        }
    }
}

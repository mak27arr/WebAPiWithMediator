namespace UserService.Application.DTOs
{
    public class UserProfileDto
    {
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

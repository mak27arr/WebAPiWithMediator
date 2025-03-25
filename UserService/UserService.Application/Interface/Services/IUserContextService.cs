namespace UserService.Application.Interface.Services
{
    public interface IUserContextService
    {
        string? GetUserId();
        string? GetEmail();
        string? GetUserName();
    }
}

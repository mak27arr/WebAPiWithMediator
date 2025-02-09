using MediatR;

namespace JwtAuthManager.Command
{
    public class LoginCommand : IRequest<string>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}

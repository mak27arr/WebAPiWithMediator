using JwtAuthManager.Command;
using JwtAuthManager.Interface;
using MediatR;

namespace JwtAuthManager.Handler
{
    internal class LoginHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly ITokenService _tokenService;

        public LoginHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var isAuthenticated = await _tokenService.LoginUser(request.Username, request.Password);

            if (!isAuthenticated)
                return string.Empty;

            return  await _tokenService.GenerateToken(request.Username);
        }
    }
}

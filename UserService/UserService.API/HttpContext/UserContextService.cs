using System.Security.Claims;
using UserService.API.Helper;
using UserService.Application.Interface.Services;

namespace UserService.API.HttpContext
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value ?? string.Empty;
        }

        public string GetEmail()
        {
            return _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? 
                _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Upn)?.Value ?? 
                string.Empty;
        }

        public string GetUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ??
                _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.PreferredUsername)?.Value ??
                string.Empty;
        }
    }
}

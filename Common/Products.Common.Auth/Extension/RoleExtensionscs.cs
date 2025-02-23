using Products.Common.Auth.Role;

namespace Products.Common.Auth.Extension
{
    public static class RoleExtensions
    {
        public static string ToRoleString(this UserRoles roles)
        {
            var role = Enum.GetValues<UserRoles>().Where(role => roles.HasFlag(role);
           return string.Join(",", role);
        }
    }
}

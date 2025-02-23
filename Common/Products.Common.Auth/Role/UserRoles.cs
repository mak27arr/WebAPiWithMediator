namespace Products.Common.Auth.Role
{
    [Flags]
    public enum UserRoles
    {
        None = 0,
        User = 1 << 0,
        Manager = 1 << 1,
        Logistics = 1 << 2,
        Admin = 1 << 3
    }
}

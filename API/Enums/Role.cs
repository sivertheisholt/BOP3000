using API.Enums;

namespace API.Enums
{
    public enum Role
    {
        Member,
        Premium,
        Admin,
    }
}

public static class RoleExtensions
{
    public static string MakeString(this Role role)
    {
        switch (role)
        {
            case Role.Member:
                return "Member";
            case Role.Premium:
                return "Premium";
            case Role.Admin:
                return "Admin";
            default:
                return "How did you manage this?";
        }
    }
}

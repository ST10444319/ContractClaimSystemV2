using ContractClaimSystem.Web.Models;

namespace ContractClaimSystem.Web.Services
{
    public interface IRoleContext
    {
        UserRole GetCurrentRole(HttpContext ctx);
        void SetRole(HttpContext ctx, UserRole role);
    }

    public class RoleContext : IRoleContext
    {
        private const string CookieName = "demo_role";

        public UserRole GetCurrentRole(HttpContext ctx)
        {
            if (ctx.Request.Cookies.TryGetValue(CookieName, out var val) &&
                Enum.TryParse<UserRole>(val, out var role))
            {
                return role;
            }

            return UserRole.Lecturer;
        }

        public void SetRole(HttpContext ctx, UserRole role)
        {
            ctx.Response.Cookies.Append(
                CookieName,
                role.ToString(),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                }
            );
        }
    }
}

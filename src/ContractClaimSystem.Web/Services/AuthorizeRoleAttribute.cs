using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ContractClaimSystem.Web.Models;

namespace ContractClaimSystem.Web.Services
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeRoleAttribute : Attribute, IAsyncActionFilter
    {
        private readonly HashSet<UserRole> _allowed;

        public AuthorizeRoleAttribute(params UserRole[] allowed)
        {
            _allowed = allowed.ToHashSet();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var roleCtx = context.HttpContext.RequestServices.GetRequiredService<IRoleContext>();
            var role = roleCtx.GetCurrentRole(context.HttpContext);

            if (!_allowed.Contains(role))
            {
                if (context.Controller is Controller c)
                    c.TempData["Error"] = "You are not authorized to perform this action.";

                context.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }

            await next();
        }
    }
}

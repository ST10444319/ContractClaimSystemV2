using Microsoft.AspNetCore.Mvc;
using ContractClaimSystem.Web.Models;
using ContractClaimSystem.Web.Services;

namespace ContractClaimSystem.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleContext _rc;

        public RoleController(IRoleContext rc)
        {
            _rc = rc;
        }

        [HttpPost]
        public IActionResult Set(UserRole role, string? returnUrl = null)
        {
            _rc.SetRole(HttpContext, role);
            TempData["Success"] = $"Switched role to {role} (demo).";

            return Redirect(returnUrl ?? Url.Action("Index", "Home")!);
        }
    }
}

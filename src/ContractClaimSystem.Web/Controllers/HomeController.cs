using Microsoft.AspNetCore.Mvc;
using ContractClaimSystem.Web.Data;
using Microsoft.EntityFrameworkCore;
using ContractClaimSystem.Web.Services;
using ContractClaimSystem.Web.Models;

namespace ContractClaimSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IRoleContext _rc;

        public HomeController(AppDbContext db, IRoleContext rc)
        {
            _db = db;
            _rc = rc;
        }

        public async Task<IActionResult> Index()
        {
            var pending = await _db.Claims.CountAsync(c => c.Status == ClaimStatus.Pending);
            var approved = await _db.Claims.CountAsync(c => c.Status == ClaimStatus.Approved);
            var rejected = await _db.Claims.CountAsync(c => c.Status == ClaimStatus.Rejected);
            var submitted = await _db.Claims.CountAsync();

            ViewBag.Pending = pending;
            ViewBag.Approved = approved;
            ViewBag.Rejected = rejected;
            ViewBag.Submitted = submitted;
            ViewBag.CurrentRole = _rc.GetCurrentRole(HttpContext);

            return View();
        }
    }
}

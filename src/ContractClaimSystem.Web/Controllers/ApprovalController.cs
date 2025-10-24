using Microsoft.AspNetCore.Mvc; using ContractClaimSystem.Web.Data; using ContractClaimSystem.Web.Models; using Microsoft.EntityFrameworkCore; using ContractClaimSystem.Web.Services; 

namespace ContractClaimSystem.Web.Controllers 
{
    [AuthorizeRole(UserRole.Coordinator, UserRole.Manager)]
    public class ApprovalController : Controller 
    { private readonly AppDbContext _db; 
        public ApprovalController(AppDbContext db) => _db = db;
        [HttpGet] public async Task<IActionResult> Index() 
        { 
            var pending = await _db.Claims 
                .Where(c => c.Status == ClaimStatus.Pending || c.Status == ClaimStatus.Verified) 
                .OrderBy(c => c.CreatedAt) .ToListAsync();
            return View(pending);
        }


        [HttpPost] [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Approve(int id) { var claim = await _db.Claims.FindAsync(id); if (claim == null) return NotFound(); claim.Status = ClaimStatus.Approved; _db.Approvals.Add(new Approval { ClaimId = id, ApproverName = "Demo Approver", Role = "Coordinator", Decision = "Approved" }); await _db.SaveChangesAsync(); TempData["Success"] = $"Claim #{id} approved."; return RedirectToAction(nameof(Index)); } [HttpPost] [ValidateAntiForgeryToken] public async Task<IActionResult> Reject(int id) { var claim = await _db.Claims.FindAsync(id); if (claim == null) return NotFound(); claim.Status = ClaimStatus.Rejected; _db.Approvals.Add(new Approval { ClaimId = id, ApproverName = "Demo Approver", Role = "Coordinator", Decision = "Rejected" }); await _db.SaveChangesAsync(); TempData["Success"] = $"Claim #{id} rejected."; return RedirectToAction(nameof(Index)); } } }
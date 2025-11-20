using Microsoft.AspNetCore.Mvc;
using ContractClaimSystem.Web.Data;
using ContractClaimSystem.Web.Models;
using ContractClaimSystem.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace ContractClaimSystem.Web.Controllers
{
    [AuthorizeRole(UserRole.HR)]
    public class HRController : Controller
    {
        private readonly AppDbContext _db;

        public HRController(AppDbContext db)
        {
            _db = db;
        }

        // HR dashboard
        public async Task<IActionResult> Index()
        {
            // Get all claims into memory first
            var claims = await _db.Claims.ToListAsync();

            // Group by lecturer name – each distinct name = one row
            var summary = claims
                .GroupBy(c => (c.LecturerName ?? string.Empty).Trim())
                .Select(g => new HrSummaryRow
                {
                    LecturerName = g.Key,
                    TotalClaims = g.Count(),
                    ApprovedClaims = g.Count(c => c.Status == ClaimStatus.Approved),
                    RejectedClaims = g.Count(c => c.Status == ClaimStatus.Rejected),
                    TotalApprovedAmount = g
                        .Where(c => c.Status == ClaimStatus.Approved)
                        .Sum(c => c.TotalAmount)
                })
                .OrderBy(x => x.LecturerName)
                .ToList();

            return View(summary);
        }


        // Simple CSV export of approved claims – “report” for payments
        public async Task<FileResult> ExportApproved()
        {
            var claims = await _db.Claims
                .Where(c => c.Status == ClaimStatus.Approved)
                .OrderBy(c => c.LecturerName)
                .ToListAsync();

            var lines = new List<string> { "Lecturer,Hours,Rate,Total,CreatedAt" };
            foreach (var c in claims)
            {
                lines.Add($"{c.LecturerName},{c.HoursWorked},{c.HourlyRate},{c.TotalAmount},{c.CreatedAt:yyyy-MM-dd}");
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(string.Join("\n", lines));
            return File(bytes, "text/csv", "ApprovedClaims.csv");
        }

        // Simple class for the view
        public class HrSummaryRow
        {
            public string LecturerName { get; set; } = string.Empty;
            public int TotalClaims { get; set; }
            public int ApprovedClaims { get; set; }
            public int RejectedClaims { get; set; }
            public decimal TotalApprovedAmount { get; set; }
        }
    }
}

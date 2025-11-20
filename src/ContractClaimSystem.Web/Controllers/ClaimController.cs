using Microsoft.AspNetCore.Mvc;
using ContractClaimSystem.Web.Data;
using ContractClaimSystem.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ContractClaimSystem.Web.Services;

namespace ContractClaimSystem.Web.Controllers
{
    public class ClaimController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UploadOptions _opts;
        private readonly IWebHostEnvironment _env;
        private readonly IRoleContext _rc;

        public ClaimController(
            AppDbContext db,
            IOptions<UploadOptions> opts,
            IWebHostEnvironment env,
            IRoleContext rc)
        {
            _db = db;
            _opts = opts.Value;
            _env = env;
            _rc = rc;
        }

        [HttpGet]
        public IActionResult Submit() => View(new Claim());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Claim model, List<IFormFile> files)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Please correct the highlighted errors.";
                    return View(model);
                }

                model.Status = ClaimStatus.Pending;

                RunAutoValidation(model);

                _db.Claims.Add(model);
                await _db.SaveChangesAsync();

                if (files != null && files.Count > 0)
                {
                    var uploadRoot = Path.Combine(_env.ContentRootPath, "wwwroot/uploads");
                    Directory.CreateDirectory(uploadRoot);

                    foreach (var f in files)
                    {
                        if (f.Length == 0) continue;

                        var ext = Path.GetExtension(f.FileName).ToLowerInvariant();
                        var allowed = new[] { ".pdf", ".docx", ".xlsx" };

                        if (!allowed.Contains(ext))
                            throw new InvalidOperationException($"File type not allowed: {ext}");

                        if (f.Length > 5 * 1024 * 1024)
                            throw new InvalidOperationException($"File too large: {f.FileName}");

                        var safeName = Path.GetFileNameWithoutExtension(f.FileName);
                        var fileName = $"{safeName}_{Guid.NewGuid():N}{ext}";
                        var fullPath = Path.Combine(uploadRoot, fileName);

                        using var stream = System.IO.File.Create(fullPath);
                        await f.CopyToAsync(stream);

                        _db.Documents.Add(new Document
                        {
                            ClaimId = model.Id,
                            FileName = f.FileName,
                            FilePath = $"/uploads/{fileName}"
                        });
                    }

                    await _db.SaveChangesAsync();
                }

                TempData["Success"] = "Claim submitted successfully.";
                return RedirectToAction("Status", new { id = model.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Status(int id)
        {
            var claim = await _db.Claims
                .Include(c => c.Documents)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (claim == null)
                return NotFound();

            return View(claim);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var claims = await _db.Claims
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(claims);
        }

        private void RunAutoValidation(Claim claim)
        {
            // Simple rule examples – adjust to match your institution’s policy
            var issues = new List<string>();

            if (claim.HoursWorked > 160)
                issues.Add("Hours worked exceed the normal monthly limit (160).");

            if (claim.HourlyRate < 100 || claim.HourlyRate > 800)
                issues.Add("Hourly rate is outside the expected range (100–800).");

            if (string.IsNullOrWhiteSpace(claim.Notes))
                issues.Add("No description / notes provided for this claim.");

            claim.IsAutoFlagged = issues.Any();
            claim.AutoValidationNotes = issues.Any()
                ? string.Join(" ", issues)
                : "No automatic issues detected.";
        }

    }

}

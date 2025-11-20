using ContractClaimSystem.Web.Models;

namespace ContractClaimSystem.Web.Data
{
    public static class SeedData
    {
        public static void Initialize(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Seed demo users if none exist
            if (!db.Users.Any())
            {
                db.Users.AddRange(
                    new AppUser
                    {
                        FullName = "Mike Masinga",
                        Email = "Mike@gmail.com",
                        Role = UserRole.Lecturer
                    },
                    new AppUser
                    {
                        FullName = "Lungile Ngwenya",
                        Email = "LNgwenya@gmail.com",
                        Role = UserRole.Coordinator
                    },
                    new AppUser
                    {
                        FullName = "Brown Mokgotsi",
                        Email = "MokgotsiB@gmail.com",
                        Role = UserRole.Manager
                    },
                    new AppUser
                    {
                        FullName = "Jan Van De Merwe",
                        Email = "J.DeMerwe@gmail.com",
                        Role = UserRole.HR
                    }
                );
            }

            // Seed demo claims if none exist
            if (!db.Claims.Any())
            {
                db.Claims.AddRange(
                    new Claim
                    {
                        LecturerName = "Brown Mokgotsi",
                        HoursWorked = 8,
                        HourlyRate = 500,
                        Notes = "CS101 Lectures",
                        Status = ClaimStatus.Pending
                    },
                    new Claim
                    {
                        LecturerName = "Shadrek Sibiya",
                        HoursWorked = 5,
                        HourlyRate = 450,
                        Notes = "Tutorials",
                        Status = ClaimStatus.Approved
                    },
                    new Claim
                    {
                        LecturerName = "General Mkwanazi",
                        HoursWorked = 6,
                        HourlyRate = 450,
                        Notes = "Labs",
                        Status = ClaimStatus.Rejected
                    }
                );
            }

            db.SaveChanges();
        }
    }
}

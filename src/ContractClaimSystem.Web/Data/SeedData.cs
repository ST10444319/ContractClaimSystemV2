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
                        FullName = "Demo Lecturer",
                        Email = "lecturer@demo.local",
                        Role = UserRole.Lecturer
                    },
                    new AppUser
                    {
                        FullName = "Demo Coordinator",
                        Email = "coord@demo.local",
                        Role = UserRole.Coordinator
                    },
                    new AppUser
                    {
                        FullName = "Demo Manager",
                        Email = "manager@demo.local",
                        Role = UserRole.Manager
                    }
                );
            }

            // Seed demo claims if none exist
            if (!db.Claims.Any())
            {
                db.Claims.AddRange(
                    new Claim
                    {
                        LecturerName = "Demo Lecturer",
                        HoursWorked = 8,
                        HourlyRate = 500,
                        Notes = "CS101 Lectures",
                        Status = ClaimStatus.Pending
                    },
                    new Claim
                    {
                        LecturerName = "Demo Lecturer",
                        HoursWorked = 5,
                        HourlyRate = 450,
                        Notes = "Tutorials",
                        Status = ClaimStatus.Approved
                    },
                    new Claim
                    {
                        LecturerName = "Demo Lecturer",
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

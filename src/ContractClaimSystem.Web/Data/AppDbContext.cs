using Microsoft.EntityFrameworkCore;
using ContractClaimSystem.Web.Models;

namespace ContractClaimSystem.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Claim> Claims => Set<Claim>();
        public DbSet<Document> Documents => Set<Document>();
        public DbSet<Approval> Approvals => Set<Approval>();
        public DbSet<AppUser> Users => Set<AppUser>();
    }
}

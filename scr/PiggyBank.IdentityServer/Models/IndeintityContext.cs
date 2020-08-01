using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PiggyBank.IdentityServer.Models
{
    public class IndeintityContext : IdentityDbContext<ApplicationUser>
    {
        public IndeintityContext(DbContextOptions options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users", "Idt");
            builder.Entity<IdentityRole>().ToTable("Roles", "Idt");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Idt");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Idt");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Idt");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Idt");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Idt");
        }
    }

    public class ApplicationUser : IdentityUser
    {
        public string CurrencyBase { get; set; }
    }

    public class IndeintityContextFactory : IDesignTimeDbContextFactory<IndeintityContext>
    {
        public IndeintityContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<IndeintityContext>();
            builder.UseSqlServer("Data Source=SQL5050.site4now.net;Initial Catalog=DB_A63631_trest;User Id=DB_A63631_trest_admin;Password=sceby7imRCXK8hu;");
            return new IndeintityContext(builder.Options);
        }
    }
}

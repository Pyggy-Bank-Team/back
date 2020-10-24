using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PiggyBank.IdentityServer.Models
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
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

    public class IdentityContextFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<IdentityContext>();
            builder.UseSqlServer("");
            return new IdentityContext(builder.Options);
        }
    }
}

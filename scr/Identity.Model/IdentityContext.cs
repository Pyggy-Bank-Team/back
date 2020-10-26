using Identity.Model.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Model
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
}
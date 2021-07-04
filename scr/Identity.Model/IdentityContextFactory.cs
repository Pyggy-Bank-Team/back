using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.Model
{
    public class IdentityContextFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<IdentityContext>();
            builder.UseSqlServer("Data Source=DESKTOP-4FJNSDH;Initial Catalog=PiggyBank;Integrated Security=True");
            return new IdentityContext(builder.Options);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PiggyBank.Model
{
    public class PiggyContextFactory : IDesignTimeDbContextFactory<PiggyContext>
    {
        public PiggyContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PiggyContext>();
            builder.UseSqlServer("");
            return new PiggyContext(builder.Options);
        }
    }
}

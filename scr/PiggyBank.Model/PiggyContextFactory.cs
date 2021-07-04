using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PiggyBank.Model
{
    public class PiggyContextFactory : IDesignTimeDbContextFactory<PiggyContext>
    {
        public PiggyContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PiggyContext>();
            builder.UseSqlServer(@"Data Source=DESKTOP-4FJNSDH;Initial Catalog=PiggyBank;Integrated Security=True");
            return new PiggyContext(builder.Options);
        }
    }
}

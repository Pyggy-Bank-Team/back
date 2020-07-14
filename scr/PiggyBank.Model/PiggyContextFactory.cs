using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PiggyBank.Model
{
    public class PiggyContextFactory : IDesignTimeDbContextFactory<PiggyContext>
    {
        public PiggyContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PiggyContext>();
            builder.UseSqlServer("Data Source=SQL5050.site4now.net;Initial Catalog=DB_A63631_trest;User Id=DB_A63631_trest_admin;Password=sceby7imRCXK8hu;");
            return new PiggyContext(builder.Options);
        }
    }
}

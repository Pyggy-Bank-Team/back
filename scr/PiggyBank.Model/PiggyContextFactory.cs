using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PiggyBank.Model
{
    public class PiggyContextFactory : IDesignTimeDbContextFactory<PiggyContext>
    {
        public PiggyContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PiggyContext>();
            builder.UseSqlServer("workstation id=piggy-pumba.mssql.somee.com;packet size=4096;user id=trest333_SQLLogin_1;pwd=s7mntjv5tv;data source=piggy-pumba.mssql.somee.com;persist security info=False;initial catalog=piggy-pumba");
            return new PiggyContext(builder.Options);
        }
    }
}

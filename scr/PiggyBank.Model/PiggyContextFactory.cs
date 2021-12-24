using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PiggyBank.Model
{
    public class PiggyContextFactory : IDesignTimeDbContextFactory<PiggyContext>
    {
        public PiggyContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PiggyContext>();
            builder.UseSqlServer("Data Source=wpl43.hosting.reg.ru; Database=u1210056_back_pumba; Integrated Security=False; User ID=u1210056_back; Password=CiHvxecr!zTax$)icA*Rwy@Nm2$kmL!h");
            return new PiggyContext(builder.Options);
        }
    }
}

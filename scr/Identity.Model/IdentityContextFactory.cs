using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.Model
{
    public class IdentityContextFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<IdentityContext>();
            builder.UseSqlServer("Data Source=wpl43.hosting.reg.ru; Database=u1210056_back_pumba; Integrated Security=False; User ID=u1210056_back; Password=CiHvxecr!zTax$)icA*Rwy@Nm2$kmL!h");
            return new IdentityContext(builder.Options);
        }
    }
}
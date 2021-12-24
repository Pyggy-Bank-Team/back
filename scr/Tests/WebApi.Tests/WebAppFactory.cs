using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PiggyBank.Model;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Tests
{
    public class WebAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var piggyBankContext = services.SingleOrDefault(s => s.ServiceType == typeof(PiggyContext));
                services.Remove(piggyBankContext);

                services.AddDbContext<PiggyContext>(options => options.UseInMemoryDatabase("PiggyContext"));
            });
        }
    }
}
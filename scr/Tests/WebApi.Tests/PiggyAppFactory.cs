using System.Linq;
using Identity.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PiggyBank.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace WebApi.Tests
{
    public class PiggyAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        // protected override IHostBuilder CreateHostBuilder()
        //     => Host.CreateDefaultBuilder()
        //         .UseEnvironment("Test")
        //         .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<TStartup>(); });

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // var piggyBankContext = services.SingleOrDefault(s => s.ServiceType == typeof(PiggyContext));
                // services.Remove(piggyBankContext);
                //
                // services.AddDbContext<PiggyContext>(options => options.UseInMemoryDatabase("PiggyContext"), ServiceLifetime.Scoped);

                var identityContext = services.SingleOrDefault(s => s.ServiceType == typeof(IdentityContext));
                services.Remove(identityContext);

                services.AddDbContext<IdentityContext>(options => options.UseInMemoryDatabase("IdentityContext"), ServiceLifetime.Scoped);
            });
        }
    }
}
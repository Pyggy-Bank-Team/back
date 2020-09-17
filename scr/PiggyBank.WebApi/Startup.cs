using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PiggyBank.Common.Interfaces;
using PiggyBank.Domain.Infrastructure;
using PiggyBank.Domain.Services;
using PiggyBank.IdentityServer.Models;

namespace PiggyBank.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddTransient(x => new ServiceSettings
            {
                ConnectionString = @"workstation id=piggy-pumba.mssql.somee.com;packet size=4096;user id=trest333_SQLLogin_1;pwd=s7mntjv5tv;data source=piggy-pumba.mssql.somee.com;persist security info=False;initial catalog=piggy-pumba"
            });
            services.AddScoped<IAccountService, PiggyService>();
            services.AddScoped<ICategoryService, PiggyService>();
            services.AddScoped<IOperationService, PiggyService>();
            
            //TODO: Make up a sane interaction with UserManager
            services.AddDbContext<IndeintityContext>(opt =>
                opt.UseSqlServer(@"workstation id=piggy-pumba.mssql.somee.com;packet size=4096;user id=trest333_SQLLogin_1;pwd=s7mntjv5tv;data source=piggy-pumba.mssql.somee.com;persist security info=False;initial catalog=piggy-pumba"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PiggyBank API", Version = "v1" });

                var scheme = new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                };

                c.AddSecurityDefinition("Bearer", scheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                          },
                            new string[] {}
                    }
                });
            });

            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "http://piggy-identity.somee.com/";
                options.RequireHttpsMetadata = false;
                options.Audience = "api1";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PiggyBank V1");
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

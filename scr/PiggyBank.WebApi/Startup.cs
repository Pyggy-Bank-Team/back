using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PiggyBank.Common.Interfaces;
using PiggyBank.Domain.Infrastructure;
using PiggyBank.Domain.Services;
using PiggyBank.IdentityServer.Models;
using PiggyBank.WebApi.Configs;
using PiggyBank.WebApi.Extensions;
using PiggyBank.WebApi.Interfaces;
using PiggyBank.WebApi.Services;

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
            services.AddScoped<IDashboardService, PiggyService>();
            services.AddScoped<ITokenResponseService, TokenResponseService>();
            
            // services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            // {
            //     opt.Password.RequireDigit = false;
            //     opt.Password.RequireLowercase = false;
            //     opt.Password.RequireNonAlphanumeric = false;
            //     opt.Password.RequireUppercase = false;
            // }).AddEntityFrameworkStores<IdentityContext>();
            
            // Hosting doesn't add IHttpContextAccessor by default
            services.AddHttpContextAccessor();
            // Identity services
            services.TryAddScoped<IUserValidator<ApplicationUser>, UserValidator<ApplicationUser>>();
            services.TryAddScoped<IPasswordValidator<ApplicationUser>, PasswordValidator<ApplicationUser>>();
            services.TryAddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            // No interface for the error describer so we can add errors without rev'ing the interface
            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<UserManager<ApplicationUser>>();
            services.AddStore<IdentityContext>(typeof(ApplicationUser));
            services.AddDbContext<IdentityContext>(opt =>
                opt.UseSqlServer(@"workstation id=piggy-pumba.mssql.somee.com;packet size=4096;user id=trest333_SQLLogin_1;pwd=s7mntjv5tv;data source=piggy-pumba.mssql.somee.com;persist security info=False;initial catalog=piggy-pumba"));
          

            //TODO: Make up a sane interaction with UserManager

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
            
            var tokenConfigs = new TokenConfigs();
            Configuration.GetSection(TokenConfigs.SectionName).Bind(tokenConfigs);

            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.ClaimsIssuer = tokenConfigs.Issuer;
                options.RequireHttpsMetadata = false;
                options.Audience = tokenConfigs.Audience;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = tokenConfigs.Issuer,
                    ValidateAudience = true,
                    ValidAudience = tokenConfigs.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenConfigs.ClientSecret))
                };;
            });

            services.Configure<TokenConfigs>(Configuration.GetSection(TokenConfigs.SectionName));
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

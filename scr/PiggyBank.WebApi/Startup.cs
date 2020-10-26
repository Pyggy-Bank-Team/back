using System.Text;
using Identity.Model;
using Identity.Model.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PiggyBank.Common.Interfaces;
using PiggyBank.Domain.Services;
using PiggyBank.Model;
using PiggyBank.WebApi.Extensions;
using PiggyBank.WebApi.Interfaces;
using PiggyBank.WebApi.Options;
using PiggyBank.WebApi.Services;

namespace PiggyBank.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            services.AddScoped<IAccountService, PiggyService>();
            services.AddScoped<ICategoryService, PiggyService>();
            services.AddScoped<IOperationService, PiggyService>();
            services.AddScoped<IDashboardService, PiggyService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddIdentityServices<ApplicationUser>();
            services.AddStore<IdentityContext>(typeof(ApplicationUser));

            var connectionString = Configuration.GetConnectionString("PumbaDb");

            services.AddDbContext<IdentityContext>(opt => opt.UseSqlServer(connectionString));
            services.AddDbContext<PiggyContext>(options => options.UseSqlServer(connectionString));

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "PiggyBank API", Version = "v1"});

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
                        new string[] { }
                    }
                });
            });
            #endregion

            var tokenOptions = Configuration.GetSection(TokenOptions.SectionName);

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.ClaimsIssuer = tokenOptions["Issuer"];
                    options.RequireHttpsMetadata = false;
                    options.Audience = tokenOptions["Audience"];
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = tokenOptions["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = tokenOptions["Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenOptions["ClientSecret"])),
                        ValidateLifetime = true
                    };
                });

            services.Configure<TokenOptions>(Configuration.GetSection(TokenOptions.SectionName));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "PiggyBank V1"); });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
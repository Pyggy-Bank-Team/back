using System.Text;
using FluentValidation;
using Identity.Model;
using Identity.Model.Models;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PiggyBank.Domain;
using PiggyBank.Domain.Behaviours;
using PiggyBank.Domain.Helpers;
using PiggyBank.Model;
using PiggyBank.Model.Interfaces;
using PiggyBank.Model.Repositories;
using PiggyBank.WebApi.Extensions;
using PiggyBank.WebApi.Filters;
using PiggyBank.WebApi.Interfaces;
using PiggyBank.WebApi.Middlewares;
using PiggyBank.WebApi.Options;
using PiggyBank.WebApi.Services;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer.Options;
using Telegram.Bot;

namespace PiggyBank.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
            => (Configuration, Environment) = (configuration, environment);

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true)
                .AddNewtonsoftJson();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ValidateRequestAttribute>();
            services.AddMediatR(typeof(EntryPoint).Assembly);

            services.AddIdentityServices<ApplicationUser>();
            services.AddStore<IdentityContext>(typeof(ApplicationUser));

            var connectionString = Configuration.GetConnectionString("PumbaDb");

            services.AddDbContext<IdentityContext>(opt => opt.UseSqlServer(connectionString), ServiceLifetime.Transient);
            services.AddDbContext<PiggyContext>(opt => opt.UseSqlServer(connectionString), ServiceLifetime.Transient);

            #region Swagger

            if (Environment.IsDevelopment())
            {
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
                            new string[] { }
                        }
                    });
                });
            }

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
                        ValidateLifetime = false
                    };
                });

            services.Configure<TokenOptions>(Configuration.GetSection(TokenOptions.SectionName));

            var options = new SinkOptions
            {
                TableName = "Store",
                SchemaName = "Audit",
                AutoCreateSqlTable = true
            };

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.MSSqlServer(connectionString, options).CreateLogger();

            services.AddSingleton(Log.Logger);

            var botOptions = Configuration.GetSection(BotOptions.BotSection);
            services.AddHttpClient("tmbot")
                .AddTypedClient<ITelegramBotClient>(client => new TelegramBotClient(botOptions["Token"], client));

            services.Configure<BotOptions>(Configuration.GetSection(BotOptions.BotSection));
            services.AddHostedService<ConfigureWebHookService>();

            services.AddMediatR(typeof(PiggyBank.Domain.EntryPoint).Assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            services.AddValidatorsFromAssembly(typeof(PiggyBank.Domain.EntryPoint).Assembly);
            services.AddScoped<ILanguageHelper, LanguageHelper>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "PiggyBank V1"); });
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
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
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using PiggyBank.Common.Commands.Accounts;
using PiggyBank.Domain;
using PiggyBank.Domain.PipelineBehaviours;
using PiggyBank.Domain.Validators.Accounts;
using PiggyBank.Model;
using PiggyBank.WebApi.Extensions;
using PiggyBank.WebApi.Factories;
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

            // services.AddScoped<IAccountService, AccountService>();
            // services.AddScoped<ICategoryService, CategoryService>();
            // services.AddScoped<IOperationService, OperationService>();
            // services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IItemFactory, ItemFactory>();
            // services.AddScoped<IBotService, BotService>();
            services.AddScoped<InvalidState>();
            services.AddMediatR(typeof(MediatREntryPoint).Assembly);

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

            services.AddMediatR(typeof(PiggyBank.Domain.MediatREntryPoint).Assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorPipelineBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

            services.AddScoped<IValidator<AddAccountCommand>, AddAccountCommandValidator>();
            services.AddScoped<IValidator<ArchiveAccountCommand>, ArchiveAccountCommandValidator>();

            services.AddScoped<IValidator<UpdateAccountCommand>, UpdateAccountCommandValidator>();
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
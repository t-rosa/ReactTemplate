using FluentValidation;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ReactTemplate.Server.Extensions;
using ReactTemplate.Server.Modules.Email;
using ReactTemplate.Server.Modules.Users;

namespace ReactTemplate.Server.Extensions;

public static class WebApplicationBuilderExtensions
{
    extension(WebApplicationBuilder builder)
    {
        public WebApplicationBuilder AddControllers()
        {
            builder.Services.AddControllers();

            builder.Services.AddOpenApi();

            builder.Services.AddResponseCompression();

            return builder;
        }

        public WebApplicationBuilder AddDatabase()
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseNpgsql(
                        builder.Configuration["CONNECTION_STRING"]
                    )
                    .UseSnakeCaseNamingConvention();
            });
            return builder;
        }

        public WebApplicationBuilder AddObservability()
        {
            builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
                .WithTracing(tracing => tracing
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation())
                .WithMetrics(metrics => metrics
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation())
                .UseOtlpExporter();

            builder.Logging.AddOpenTelemetry(options =>
            {
                options.IncludeScopes = true;
                options.IncludeFormattedMessage = true;
            });

            return builder;
        }

        public WebApplicationBuilder AddAuthenticationServices()
        {
            builder.Services.AddAuthorization();

            builder.Services
                .AddIdentityApiEndpoints<User>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.SignIn.RequireConfirmedEmail = true;
                    options.User.RequireUniqueEmail = true;
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            });

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo("./keys/storage"));
            }
            else
            {
                builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo("keys/storage"));
            }

            return builder;
        }

        public WebApplicationBuilder AddEmailServices()
        {
            builder.Services.AddTransient<IEmailSender<User>, EmailSender>();
            builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("SmtpOptions"));

            return builder;
        }

        public WebApplicationBuilder AddApplicationServices()
        {
            builder.Services.AddValidatorsFromAssemblyContaining<IProgram>();

            builder.Services.AddHttpContextAccessor();

            return builder;
        }
    }
}

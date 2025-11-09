using FluentValidation;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ReactTemplate.Server.Modules.Email;
using ReactTemplate.Server.Modules.Users;
using Scalar.AspNetCore;

namespace ReactTemplate.Server;

public interface IProgram
{
    private static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddResponseCompression();

        builder.Services.AddSingleton<ISystemClock, SystemClock>();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options
                .UseNpgsql(builder.Configuration["CONNECTION_STRING"])
                .UseSnakeCaseNamingConvention();
        });

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

        builder.Services.AddTransient<IEmailSender<User>, EmailSender>();

        builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("SmtpOptions"));

        builder.Services.AddValidatorsFromAssembly(typeof(IProgram).Assembly, includeInternalTypes: true);

        builder.Services.AddControllers();

        builder.Services.AddOpenApi();

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

        WebApplication app = builder.Build();

        app.UseResponseCompression();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();

            using IServiceScope scope = app.Services.CreateScope();
            ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        await SeedData.ExecuteAsync(app.Services, builder.Configuration);
        if (app.Environment.IsDevelopment())
        {
            await SeedTestData.ExecuteAsync(app.Services, builder.Configuration);
        }

        await app.RunAsync();
    }
}

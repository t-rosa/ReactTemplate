using FluentValidation;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ReactTemplate.Server.Modules.Email;
using ReactTemplate.Server.Modules.Users;
using Scalar.AspNetCore;

namespace ReactTemplate.Server;

public interface Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Open Telemetry
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("ReactTemplate"))
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                metrics.AddOtlpExporter();
            })
            .WithTracing((tracing) =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation();

                tracing.AddOtlpExporter();
            });

        builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());

        builder.Services.Configure<OtlpExporterOptions>(options =>
        {
            options.Headers = $"x-otlp-api-key={builder.Configuration["Otlp:Key"]}";
        });

        // Database
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options
                .UseNpgsql(builder.Configuration["Database:ReactTemplate:ConnectionString"])
                .UseSnakeCaseNamingConvention();
        });

        // Authentication
        builder.Services.AddAuthorization();

        builder.Services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<ApplicationDbContext>();

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

        // Email
        builder.Services.AddTransient<IEmailSender, EmailSender>();

        builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("SmtpOptions"));

        // Validation
        builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

        builder.Services.AddControllers();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();

            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.MapIdentityApi<User>();
        app.MapPost("/logout", async (SignInManager<User> signInManager,
            [FromBody] object empty) =>
        {
            if (empty != null)
            {
                await signInManager.SignOutAsync();
                return Results.Ok();
            }
            return Results.Unauthorized();
        })
        .WithOpenApi()
        .RequireAuthorization();

        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}

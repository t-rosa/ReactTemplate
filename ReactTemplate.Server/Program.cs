using FluentValidation;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReactTemplate.Server.Modules.Email;
using ReactTemplate.Server.Modules.Users;
using Scalar.AspNetCore;

namespace ReactTemplate.Server;

public interface Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // HttpContextAccessor to access the current user in DbContext
        builder.Services.AddHttpContextAccessor();

        // Database
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options
                .UseNpgsql(builder.Configuration["CONNECTION_STRING"])
                .UseSnakeCaseNamingConvention();
        });

        // Authentication
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

        // Email
        builder.Services.AddTransient<IEmailSender<User>, EmailSender>();

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

        app.MapFallbackToFile("/index.html");

        await SeedData.ExecuteAsync(app.Services, builder.Configuration);
        if (app.Environment.IsDevelopment())
        {
            await SeedTestData.ExecuteAsync(app.Services, builder.Configuration);
        }
        app.Run();
    }
}

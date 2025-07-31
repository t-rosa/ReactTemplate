using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ReactTemplate.Authentication;

public static class AuthenticationServiceExtensions
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, ConfigurationManager configuration, IHostEnvironment environment)
    {
        services.AddDbContext<AuthenticationContext>(options =>
            options.UseNpgsql(configuration["Database:ReactTemplate:ConnectionString"])
                    .UseSnakeCaseNamingConvention()
        );

        services.AddAuthorization();

        services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<AuthenticationContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
        });

        return services;
    }

    public static void ApplyAuthenticationMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AuthenticationContext>();
        context.Database.Migrate();
    }
}

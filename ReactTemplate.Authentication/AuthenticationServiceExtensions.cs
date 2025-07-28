using Microsoft.AspNetCore.DataProtection;
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

        services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<AuthenticationContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
        });

        if (environment.IsDevelopment())
        {
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo("./keys/storage"));
        }
        else
        {
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo("keys/storage"));
        }

        return services;
    }
}

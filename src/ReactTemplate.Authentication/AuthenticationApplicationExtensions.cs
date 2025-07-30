using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ReactTemplate.Authentication;

public static class AuthenticationApplicationExtensions
{
    public static void ApplyAuthenticationMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AuthenticationContext>();
        context.Database.Migrate();
    }
}

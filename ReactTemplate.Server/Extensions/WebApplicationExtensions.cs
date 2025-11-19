using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReactTemplate.Server.Database;
using ReactTemplate.Server.Modules.Auth;

namespace ReactTemplate.Server.Extensions;

public static class WebApplicationExtensions
{
    extension(WebApplication app)
    {
        public async Task ApplyMigrationAsync()
        {
            using IServiceScope scope = app.Services.CreateScope();
            ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                await db.Database.MigrateAsync();
                app.Logger.LogInformation("Database migrations applied successfully.");
            }
            catch (Exception e)
            {
                app.Logger.LogError(e, "An error occurred while applying database migrations.");
                throw;
            }
        }

        public async Task SeedInitialDataAsync()
        {
            using IServiceScope scope = app.Services.CreateScope();
            RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            try
            {
                if (!await roleManager.RoleExistsAsync(Roles.Member))
                {
                    await roleManager.CreateAsync(new IdentityRole(Roles.Member));
                }

                if (!await roleManager.RoleExistsAsync(Roles.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                }

                app.Logger.LogInformation("successfully created roles.");
            }
            catch (Exception exception)
            {
                app.Logger.LogError(exception, "An error occurred while seeding initial data.");
                throw;
            }
        }
    }
}


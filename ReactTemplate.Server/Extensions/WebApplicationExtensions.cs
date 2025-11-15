using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReactTemplate.Server.Modules.Users;

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

        public async Task SeedRolesAsync()
        {
            using IServiceScope scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            if (!await db.Database.CanConnectAsync())
            {
                Console.WriteLine("⚠️ Database not available, skipping seeding.");
                return;
            }

            string[] roles = ["Admin", "Member", "User"];

            foreach (var role in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                    Console.WriteLine($"✅ Role '{role}' created.");
                }
            }
        }

        public async Task SeedAdminUserAsync()
        {
            using IServiceScope scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            if (!await db.Database.CanConnectAsync())
            {
                Console.WriteLine("⚠️ Database not available, skipping seeding.");
                return;
            }

            var adminEmail = configuration["ADMIN_EMAIL"];
            var adminPassword = configuration["ADMIN_PASSWORD"];

            if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
            {
                Console.WriteLine("⚠️ ADMIN_EMAIL or ADMIN_PASSWORD not set, skipping admin creation.");
                return;
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine($"✅ Admin user '{adminEmail}' created.");
                }
                else
                {
                    Console.WriteLine($"❌ Failed to create admin: {string.Join(", ", result.Errors)}");
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine($"✅ Admin role added to '{adminEmail}'.");
                }
            }
        }

        public async Task SeedTestUsersAsync()
        {
            using IServiceScope scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            if (!await db.Database.CanConnectAsync())
            {
                Console.WriteLine("⚠️ Database not available, skipping seeding.");
                return;
            }

            var memberEmail = "member@test.com";
            var memberPassword = "MemberTest1!";

            var memberUser = await userManager.FindByEmailAsync(memberEmail);
            if (memberUser == null)
            {
                memberUser = new User
                {
                    UserName = memberEmail,
                    Email = memberEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(memberUser, memberPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(memberUser, "Member");
                    Console.WriteLine($"✅ Member user '{memberEmail}' created.");
                }
                else
                {
                    Console.WriteLine($"❌ Failed to create member: {string.Join(", ", result.Errors)}");
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(memberUser, "Member"))
                {
                    await userManager.AddToRoleAsync(memberUser, "Member");
                    Console.WriteLine($"✅ Member role added to '{memberEmail}'.");
                }
            }

            var baseEmail = "base@test.com";
            var basePassword = "BaseTest1!";

            var baseUser = await userManager.FindByEmailAsync(baseEmail);
            if (baseUser == null)
            {
                baseUser = new User
                {
                    UserName = baseEmail,
                    Email = baseEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(baseUser, basePassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(baseUser, "User");
                    Console.WriteLine($"✅ Base user '{baseEmail}' created.");
                }
                else
                {
                    Console.WriteLine($"❌ Failed to create base user: {string.Join(", ", result.Errors)}");
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(baseUser, "User"))
                {
                    await userManager.AddToRoleAsync(baseUser, "User");
                    Console.WriteLine($"✅ User role added to '{baseEmail}'.");
                }
            }
        }
    }
}


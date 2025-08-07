using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactTemplate.Server;
using ReactTemplate.Server.Modules.Users;
using Testcontainers.PostgreSql;

namespace ReactTemplate.Tests;

public class BaseFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithDatabase("react-template-test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithPortBinding(5555, 5432)
        .WithWaitStrategy(Wait.ForUnixContainer())
        .Build();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(_container.GetConnectionString())
                        .UseSnakeCaseNamingConvention());

        });

        builder.Build();

        base.ConfigureWebHost(builder);
    }

    // public static async Task SeedUsersAsync(UserManager<User> userManager)
    // {
    //     var adminEmail = "admin@example.com";
    //     var adminUser = await userManager.FindByEmailAsync(adminEmail);
    //     if (adminUser == null)
    //     {
    //         var user = new User
    //         {
    //             UserName = adminEmail,
    //             Email = adminEmail,
    //             EmailConfirmed = true
    //         };

    //         var result = await userManager.CreateAsync(user, "Admin123!");

    //         if (!result.Succeeded)
    //         {
    //             throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
    //         }
    //     }
    // }
}

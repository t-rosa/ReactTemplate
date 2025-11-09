using System.Globalization;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using ReactTemplate.Server;
using Testcontainers.PostgreSql;

namespace ReactTemplate.Tests;

public class BaseFactory : WebApplicationFactory<IProgram>, IAsyncLifetime
{
    public static TestSystemClock SystemClock { get; } = new TestSystemClock();

    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithDatabase("sbire-test")
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
            ServiceDescriptor? descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(_container.GetConnectionString())
                        .UseSnakeCaseNamingConvention());

            ServiceDescriptor systemClockDescriptor = services.Single(d => d.ServiceType == typeof(ISystemClock));
            services.Remove(systemClockDescriptor);
            services.AddSingleton<ISystemClock>(SystemClock);
        });

        base.ConfigureWebHost(builder);
    }
}

public class TestSystemClock : ISystemClock
{
    private const string Input = "2025-01-01T00:00:00Z";

    public DateTimeOffset UtcNow { get; } = DateTimeOffset.Parse(Input, CultureInfo.InvariantCulture);
}

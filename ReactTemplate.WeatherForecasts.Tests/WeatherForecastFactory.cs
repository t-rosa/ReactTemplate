using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactTemplate.Authentication;
using Testcontainers.PostgreSql;

namespace ReactTemplate.WeatherForecasts.Tests;

public class WeatherForecastFactory : WebApplicationFactory<Program>, IAsyncLifetime
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
            var authDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<AuthenticationContext>));
            if (authDescriptor is not null)
            {
                services.Remove(authDescriptor);
            }

            var weatherForecastDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<WeatherForecastContext>));
            if (weatherForecastDescriptor is not null)
            {
                services.Remove(weatherForecastDescriptor);
            }

            services.AddDbContext<AuthenticationContext>(options =>
                options.UseNpgsql(_container.GetConnectionString())
                        .UseSnakeCaseNamingConvention());

            services.AddDbContext<WeatherForecastContext>(options =>
                options.UseNpgsql(_container.GetConnectionString())
                        .UseSnakeCaseNamingConvention());

        });

        base.ConfigureWebHost(builder);
    }

}

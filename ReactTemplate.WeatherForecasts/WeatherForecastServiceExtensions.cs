using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ReactTemplate.WeatherForecasts;

public static class WeatherForecastServiceExtensions
{
    public static IServiceCollection AddWeatherForecastServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<WeatherForecastContext>(options =>
            options.UseNpgsql(configuration["Database:ReactTemplate:ConnectionString"])
                    .UseSnakeCaseNamingConvention()
        );

        return services;
    }
}

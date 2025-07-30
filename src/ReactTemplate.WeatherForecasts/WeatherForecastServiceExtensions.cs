using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ReactTemplate.WeatherForecasts;

// public interface Marker { }

public static class WeatherForecastServiceExtensions
{
    public static IServiceCollection AddWeatherForecastServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<WeatherForecastContext>(options =>
            options.UseNpgsql(configuration["Database:ReactTemplate:ConnectionString"])
                    .UseSnakeCaseNamingConvention()
        );


        services.AddValidatorsFromAssembly(typeof(WeatherForecast).Assembly, includeInternalTypes: true);

        return services;
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ReactTemplate.WeatherForecasts;

public static class WeatherForecastApplicationExtensions
{
    public static void ApplyWeatherForecastMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WeatherForecastContext>();
        context.Database.Migrate();
    }
}

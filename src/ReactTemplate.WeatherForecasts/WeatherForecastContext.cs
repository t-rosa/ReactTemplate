using Microsoft.EntityFrameworkCore;

namespace ReactTemplate.WeatherForecasts;

public class WeatherForecastContext(DbContextOptions<WeatherForecastContext> options) : DbContext(options)
{
    internal DbSet<WeatherForecast> WeatherForecasts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("weather_forecasts");

        // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

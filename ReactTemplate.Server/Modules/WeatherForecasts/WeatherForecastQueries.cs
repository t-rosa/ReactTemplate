using System.Linq.Expressions;
using ReactTemplate.Server.Modules.WeatherForecasts.DTOs;

namespace ReactTemplate.Server.Modules.WeatherForecasts;

internal static class WeatherForecastQueries
{
    public static Expression<Func<WeatherForecast, WeatherForecastResponse>> ProjectToResponse()
    {
        return forecast => new WeatherForecastResponse
        {
            Id = forecast.Id,
            Date = forecast.Date,
            TemperatureC = forecast.TemperatureC,
            Summary = forecast.Summary
        };
    }
}

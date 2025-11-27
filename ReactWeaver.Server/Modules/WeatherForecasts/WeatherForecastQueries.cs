using System.Linq.Expressions;
using ReactWeaver.Server.Modules.WeatherForecasts.DTOs;

namespace ReactWeaver.Server.Modules.WeatherForecasts;

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

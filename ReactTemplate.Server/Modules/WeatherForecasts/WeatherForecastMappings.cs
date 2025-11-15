using ReactTemplate.Server.Modules.WeatherForecasts.Dtos;

namespace ReactTemplate.Server.Modules.WeatherForecasts;

internal static class WeatherForecastMappings
{

    public static WeatherForecastResponse ToWeatherForecastResponse(this WeatherForecast weatherForecast)
    {
        return new WeatherForecastResponse
        {
            Id = weatherForecast.Id,
            Date = weatherForecast.Date,
            TemperatureC = weatherForecast.TemperatureC,
            Summary = weatherForecast.Summary
        };
    }

    public static WeatherForecast ToEntity(this CreateWeatherForecastRequest request)
    {
        return new WeatherForecast
        {
            Date = request.Date,
            TemperatureC = request.TemperatureC,
            Summary = request.Summary,
            CreatedAt = DateTime.UtcNow,
        };
    }
}

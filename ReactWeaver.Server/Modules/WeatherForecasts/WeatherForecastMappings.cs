using ReactWeaver.Server.Modules.Users;
using ReactWeaver.Server.Modules.WeatherForecasts.DTOs;

namespace ReactWeaver.Server.Modules.WeatherForecasts;

internal static class WeatherForecastMappings
{
    extension(WeatherForecast weatherForecast)
    {
        public WeatherForecastResponse ToWeatherForecastResponse()
        {
            return new WeatherForecastResponse
            {
                Id = weatherForecast.Id,
                Date = weatherForecast.Date,
                TemperatureC = weatherForecast.TemperatureC,
                Summary = weatherForecast.Summary
            };
        }

        public void UpdateFromRequest(UpdateWeatherForecastRequest request, string userId)
        {
            weatherForecast.UserId = userId;
            weatherForecast.Date = request.Date;
            weatherForecast.TemperatureC = request.TemperatureC;
            weatherForecast.Summary = request.Summary;
            weatherForecast.UpdatedAt = DateTime.UtcNow;
        }
    }

    extension(CreateWeatherForecastRequest request)
    {
        public WeatherForecast ToEntity(string userId)
        {
            return new WeatherForecast
            {
                Id = $"w_{Guid.CreateVersion7()}",
                UserId = userId,
                Date = request.Date,
                TemperatureC = request.TemperatureC,
                Summary = request.Summary,
                CreatedAt = DateTime.UtcNow,
            };
        }
    }
}

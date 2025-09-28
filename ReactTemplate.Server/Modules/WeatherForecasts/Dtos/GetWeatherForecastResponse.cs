namespace ReactTemplate.Server.Modules.WeatherForecasts.Dtos;

public record GetWeatherForecastResponse(Guid Id, DateOnly Date, int TemperatureC, string? Summary);

namespace ReactTemplate.WeatherForecast.Dtos;

public class UpdateWeatherForecastRequest
{
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
}

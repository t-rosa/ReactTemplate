namespace ReactTemplate.WeatherForecasts.Dtos;

public class CreateWeatherForecastRequest
{
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
}

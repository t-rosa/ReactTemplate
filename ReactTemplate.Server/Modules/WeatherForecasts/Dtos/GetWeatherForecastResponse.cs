namespace ReactTemplate.Server.Modules.WeatherForecasts.Dtos;

public class GetWeatherForecastResponse
{
    public Guid Id { get; set; }

    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
}


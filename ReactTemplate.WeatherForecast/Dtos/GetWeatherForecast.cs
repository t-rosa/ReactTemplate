namespace ReactTemplate.WeatherForecast.Dtos;

internal class GetWeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
}


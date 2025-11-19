namespace ReactTemplate.Server.Modules.WeatherForecasts;

public sealed class WeatherForecast
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}


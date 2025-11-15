namespace ReactTemplate.Server.Modules.WeatherForecasts.Dtos;

public sealed record WeatherForecastResponse
{
    public required Guid Id { get; init; }
    public required DateOnly Date { get; init; }
    public required int TemperatureC { get; init; }
    public string? Summary { get; init; }
}

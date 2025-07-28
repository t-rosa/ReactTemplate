using Ardalis.GuardClauses;

namespace ReactTemplate.WeatherForecast;

internal class WeatherForecast
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public DateOnly Date { get; private set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public int TemperatureC { get; private set; }

    public string? Summary { get; private set; }

    internal WeatherForecast(Guid id, DateOnly date, int temperatureC, string? summary)
    {
        Id = Guard.Against.Default(id);
        Date = Guard.Against.Null(date);
        TemperatureC = Guard.Against.Null(temperatureC);
        Summary = summary;
    }
}


using Ardalis.GuardClauses;

namespace ReactTemplate.WeatherForecast;

internal class WeatherForecast
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public DateTime Date { get; private set; } = DateTime.UtcNow;

    public int TemperatureC { get; private set; }

    public string? Summary { get; private set; }

    internal WeatherForecast(Guid id, DateTime date, int temperatureC, string? summary)
    {
        Id = Guard.Against.Default(id);
        Date = Guard.Against.NullOrOutOfSQLDateRange(date);
        TemperatureC = Guard.Against.Null(temperatureC);
        Summary = summary;
    }
}


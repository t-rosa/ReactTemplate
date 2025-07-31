using Ardalis.GuardClauses;
using ReactTemplate.Server.Modules.Users;

namespace ReactTemplate.Server.Modules.WeatherForecasts;

public class WeatherForecast
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid UserId { get; private set; }

    public DateOnly Date { get; private set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public int TemperatureC { get; private set; }

    public string? Summary { get; private set; }

    public User User { get; private set; } = null!;

    internal WeatherForecast(Guid id, Guid userId, DateOnly date, int temperatureC, string? summary)
    {
        Id = Guard.Against.Default(id);
        UserId = Guard.Against.Default(userId);
        Date = Guard.Against.Null(date);
        TemperatureC = Guard.Against.Null(temperatureC);
        Summary = summary;
    }
}


using ReactTemplate.Server.Modules.Users;

namespace ReactTemplate.Server.Modules.WeatherForecasts;

public class WeatherForecast
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }

    public User User { get; set; } = default!;
}


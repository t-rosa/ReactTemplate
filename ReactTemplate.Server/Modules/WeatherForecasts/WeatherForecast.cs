using ReactTemplate.Server.Modules.Users;

namespace ReactTemplate.Server.Modules.WeatherForecasts;

public sealed class WeatherForecast
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public User User { get; set; } = default!;
}


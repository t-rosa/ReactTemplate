using Microsoft.AspNetCore.Identity;
using ReactTemplate.Server.Modules.WeatherForecasts;

namespace ReactTemplate.Server.Modules.Users;

public class User : IdentityUser<Guid>
{
    public string? FirstName { get; private set; }

    public string? LastName { get; private set; }

    public IEnumerable<WeatherForecast> WeatherForecasts { get; } = [];
}

using Microsoft.AspNetCore.Identity;
using ReactTemplate.Server.Modules.WeatherForecasts;

namespace ReactTemplate.Server.Modules.Users;

public class User : IdentityUser<Guid>
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public ICollection<WeatherForecast> WeatherForecasts { get; set; } = default!;
}

using FluentValidation;

namespace ReactTemplate.Server.Modules.WeatherForecasts.Dtos;

public class CreateWeatherForecastRequest
{
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
}

public class CreateWeatherForecastRequestValidator : AbstractValidator<CreateWeatherForecastRequest>
{
    public CreateWeatherForecastRequestValidator()
    {
        RuleFor(e => e.Date).NotEmpty().WithMessage("La date doit être renseignée.");
        RuleFor(e => e.TemperatureC).NotEmpty().WithMessage("La température doit être renseignée.");
    }
}

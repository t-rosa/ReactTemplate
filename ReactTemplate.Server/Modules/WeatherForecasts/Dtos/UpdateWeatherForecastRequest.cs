using FluentValidation;

namespace ReactTemplate.Server.Modules.WeatherForecasts.Dtos;

public class UpdateWeatherForecastRequest
{
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
}

public class UpdateWeatherForecastRequestValidator : AbstractValidator<UpdateWeatherForecastRequest>
{
    public UpdateWeatherForecastRequestValidator()
    {
        RuleFor(e => e.Date).NotEmpty().WithMessage("La date doit être renseignée.");
        RuleFor(e => e.TemperatureC).NotEmpty().WithMessage("La température doit être renseignée.");
    }
}

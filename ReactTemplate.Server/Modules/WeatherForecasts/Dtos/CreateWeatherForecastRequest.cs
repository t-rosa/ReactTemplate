using FluentValidation;

namespace ReactTemplate.Server.Modules.WeatherForecasts.DTOs;

public sealed record CreateWeatherForecastRequest
{
    public required DateOnly Date { get; init; }
    public required int TemperatureC { get; init; }
    public string? Summary { get; init; }
}

public class CreateWeatherForecastRequestValidator : AbstractValidator<CreateWeatherForecastRequest>
{
    public CreateWeatherForecastRequestValidator()
    {
        RuleFor(e => e.Date).NotEmpty().WithMessage("Date is required.");
        RuleFor(e => e.TemperatureC).NotEmpty().WithMessage("Temperature is required.");
    }
}

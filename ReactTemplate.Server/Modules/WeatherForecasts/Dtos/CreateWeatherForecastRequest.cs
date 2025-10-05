using FluentValidation;

namespace ReactTemplate.Server.Modules.WeatherForecasts.Dtos;

public record CreateWeatherForecastRequest(int TemperatureC, string? Summary)
{
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
}

public class CreateWeatherForecastRequestValidator : AbstractValidator<CreateWeatherForecastRequest>
{
    public CreateWeatherForecastRequestValidator()
    {
        RuleFor(e => e.Date).NotEmpty().WithMessage("Date is required.");
        RuleFor(e => e.TemperatureC).NotEmpty().WithMessage("Temperature is required.");
    }
}

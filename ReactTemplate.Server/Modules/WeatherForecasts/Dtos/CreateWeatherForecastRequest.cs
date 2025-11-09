using FluentValidation;

namespace ReactTemplate.Server.Modules.WeatherForecasts.Dtos;

public class CreateWeatherForecastRequest
{
    public required DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public required int TemperatureC { get; set; }

    public string? Summary { get; set; }
}

public class CreateWeatherForecastRequestValidator : AbstractValidator<CreateWeatherForecastRequest>
{
    public CreateWeatherForecastRequestValidator()
    {
        RuleFor(e => e.Date).NotEmpty().WithMessage("Date is required.");
        RuleFor(e => e.TemperatureC).NotEmpty().WithMessage("Temperature is required.");
    }
}

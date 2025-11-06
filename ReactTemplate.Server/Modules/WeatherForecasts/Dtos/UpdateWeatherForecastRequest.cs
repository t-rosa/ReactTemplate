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
        RuleFor(e => e.Date).NotEmpty().WithMessage("Date is required.");
        RuleFor(e => e.TemperatureC).NotEmpty().WithMessage("Temperature is required.");
    }
}

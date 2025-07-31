using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReactTemplate.WeatherForecasts.Dtos;

namespace ReactTemplate.WeatherForecasts;

[ApiController]
[Route("api/weather-forecast")]
[Authorize]
public class WeatherForecastController(WeatherForecastContext context, ILogger<WeatherForecastController> logger) : ControllerBase
{
    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<GetWeatherForecastResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public IActionResult GetWeatherForecasts()
    {
        logger.LogInformation("Retrieving all weather forecasts");
        var response = context.WeatherForecasts.AsNoTracking().Select((forecast) => new GetWeatherForecastResponse
        {
            Id = forecast.Id,
            Date = forecast.Date,
            Summary = forecast.Summary,
            TemperatureC = forecast.TemperatureC
        });

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetWeatherForecastResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWeatherForecast([FromRoute] Guid id)
    {
        logger.LogInformation("Retrieving weather forecasts with ID: {ID}", id);
        var query = context.WeatherForecasts.Where(e => e.Id == id).Select((forecast) => new GetWeatherForecastResponse
        {
            Id = forecast.Id,
            Date = forecast.Date,
            Summary = forecast.Summary,
            TemperatureC = forecast.TemperatureC
        });

        var response = await query.SingleOrDefaultAsync();
        if (response is null)
        {
            logger.LogInformation("Weather forecasts with ID: {ID} not found", id);
            return NotFound();
        }

        return Ok(response);
    }

    [HttpPost()]
    [ProducesResponseType(typeof(GetWeatherForecastResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateWeatherForecast(
        [FromBody] CreateWeatherForecastRequest request,
        [FromServices] IValidator<CreateWeatherForecastRequest> validator)
    {
        logger.LogInformation("Creating weather forecast");

        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            logger.LogError("Error validating request");
            var problem = new ValidationProblemDetails(validation.ToDictionary());
            return ValidationProblem(problem);
        }

        var response = new WeatherForecast(Guid.NewGuid(), request.Date, request.TemperatureC, request.Summary);
        await context.WeatherForecasts.AddAsync(response);
        logger.LogInformation("Creating weather forecast with ID: {ID}", response.Id);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            logger.LogError("Error creating weather forecast with ID: {ID}", response.Id);
            return StatusCode(500, exception);
        }

        logger.LogInformation("Weather forecast with ID: {ID} successfully created", response.Id);
        return CreatedAtAction(nameof(GetWeatherForecast), new { id = response.Id }, response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(GetWeatherForecastResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateWeatherForecast(
        [FromRoute] Guid id,
        [FromBody] UpdateWeatherForecastRequest request,
        [FromServices] IValidator<UpdateWeatherForecastRequest> validator)
    {
        logger.LogInformation("Updating weather forecast with ID: {ID}", id);

        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            logger.LogError("Error validating request");
            var problem = new ValidationProblemDetails(validation.ToDictionary());
            return ValidationProblem(problem);
        }

        var forecast = await context.WeatherForecasts.Where(e => e.Id == id).SingleOrDefaultAsync();
        if (forecast is null)
        {
            logger.LogInformation("Weather forecasts with ID: {ID} not found", id);
            return NotFound();
        }

        context.Entry(forecast).CurrentValues.SetValues(request);
        context.Entry(forecast).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            logger.LogError("Error updating weather forecast with ID: {ID}", id);
            return StatusCode(500, exception);
        }

        logger.LogInformation("Weather forecast with ID: {ID} successfully updated", id);

        var response = context.WeatherForecasts.Where(e => e.Id == id).Select((forecast) => new GetWeatherForecastResponse
        {
            Id = forecast.Id,
            Date = forecast.Date,
            Summary = forecast.Summary,
            TemperatureC = forecast.TemperatureC
        });

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveWeatherForecast([FromRoute] Guid id)
    {
        logger.LogInformation("Removing weather forecast with ID: {ID}", id);

        var forecast = await context.WeatherForecasts.Where(e => e.Id == id).SingleOrDefaultAsync();
        if (forecast is null)
        {
            logger.LogInformation("Weather forecasts with ID: {ID} not found", id);
            return NotFound();
        }

        context.WeatherForecasts.Remove(forecast);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            logger.LogError("Error removing weather forecast with ID: {ID}", id);
            return StatusCode(500, exception);
        }

        logger.LogInformation("Weather forecast with ID: {ID} successfully removed", id);
        return NoContent();
    }
}

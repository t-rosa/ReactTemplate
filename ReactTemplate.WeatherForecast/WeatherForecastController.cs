using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactTemplate.WeatherForecast.Dtos;

namespace ReactTemplate.WeatherForecast;

[ApiController]
[Authorize]
[Route("api/weather-forecast")]
public class WeatherForecastController(WeatherForecastDbContext context) : ControllerBase
{
    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<GetWeatherForecastResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetWeatherForecasts()
    {
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWeatherForecast([FromRoute] Guid id)
    {
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
            return NotFound();
        }

        return Ok(response);
    }

    [HttpPost()]
    [ProducesResponseType(typeof(GetWeatherForecastResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateWeatherForecast([FromBody] CreateWeatherForecastRequest request)
    {
        var response = new WeatherForecast(Guid.NewGuid(), request.Date, request.TemperatureC, request.Summary);

        await context.WeatherForecasts.AddAsync(response);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(GetWeatherForecast), new { id = response.Id }, response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(GetWeatherForecastResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateWeatherForecast([FromRoute] Guid id, [FromBody] UpdateWeatherForecastRequest request)
    {
        var forecast = await context.WeatherForecasts.Where(e => e.Id == id).SingleOrDefaultAsync();

        if (forecast is null)
        {
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
            return StatusCode(500, exception);
        }

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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveWeatherForecast([FromRoute] Guid id)
    {
        var forecast = await context.WeatherForecasts.Where(e => e.Id == id).SingleOrDefaultAsync();

        if (forecast is null)
        {
            return NotFound();
        }

        context.WeatherForecasts.Remove(forecast);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            return StatusCode(500, exception);
        }

        return NoContent();
    }
}

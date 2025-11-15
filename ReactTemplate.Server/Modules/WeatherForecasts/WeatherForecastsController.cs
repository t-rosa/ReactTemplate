using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactTemplate.Server.Modules.Users;
using ReactTemplate.Server.Modules.WeatherForecasts.Dtos;

namespace ReactTemplate.Server.Modules.WeatherForecasts;

[ApiController]
[Route("api/weather-forecasts")]
[Authorize]
public sealed class WeatherForecastsController(ApplicationDbContext db, UserManager<User> userManager) : ControllerBase
{
    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<WeatherForecastResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWeatherForecasts()
    {
        var user = await userManager.GetUserAsync(HttpContext.User);
        if (user is null)
        {
            return Unauthorized();
        }

        var response = db.WeatherForecasts
            .AsNoTracking()
            .Where(e => e.UserId == user.Id)
            .Select(WeatherForecastQueries.ProjectToResponse());

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(WeatherForecastResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWeatherForecast([FromRoute] Guid id)
    {
        var user = await userManager.GetUserAsync(HttpContext.User);
        if (user is null)
        {
            return Unauthorized();
        }

        var query = db.WeatherForecasts.Where(e => e.UserId == user.Id)
                                             .Where(e => e.Id == id)
                                             .Select((forecast) => new WeatherForecastResponse
                                             {
                                                 Id = forecast.Id,
                                                 Date = forecast.Date,
                                                 TemperatureC = forecast.TemperatureC,
                                                 Summary = forecast.Summary
                                             });

        var response = await query.SingleOrDefaultAsync();
        if (response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpPost()]
    [ProducesResponseType(typeof(WeatherForecastResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateWeatherForecast(
        [FromBody] CreateWeatherForecastRequest request,
        [FromServices] IValidator<CreateWeatherForecastRequest> validator)
    {
        var user = await userManager.GetUserAsync(HttpContext.User);
        if (user is null)
        {
            return Unauthorized();
        }

        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var problem = new ValidationProblemDetails(validation.ToDictionary());
            return ValidationProblem(problem);
        }

        var forecast = new WeatherForecast()
        {
            Date = request.Date,
            Summary = request.Summary,
            TemperatureC = request.TemperatureC,
            UserId = user.Id
        };

        await db.WeatherForecasts.AddAsync(forecast);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            return StatusCode(500, exception);
        }

        var response = new WeatherForecastResponse
        {
            Id = forecast.Id,
            Date = forecast.Date,
            TemperatureC = forecast.TemperatureC,
            Summary = forecast.Summary
        };

        return CreatedAtAction(nameof(GetWeatherForecast), new { id = response.Id }, response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(WeatherForecastResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateWeatherForecast(
        [FromRoute] Guid id,
        [FromBody] UpdateWeatherForecastRequest request,
        [FromServices] IValidator<UpdateWeatherForecastRequest> validator)
    {
        var user = await userManager.GetUserAsync(HttpContext.User);
        if (user is null)
        {
            return Unauthorized();
        }

        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var problem = new ValidationProblemDetails(validation.ToDictionary());
            return ValidationProblem(problem);
        }

        var forecast = await db.WeatherForecasts.Where(e => e.UserId == user.Id)
                                                      .Where(e => e.Id == id)
                                                      .SingleOrDefaultAsync();
        if (forecast is null)
        {
            return NotFound();
        }

        db.Entry(forecast).CurrentValues.SetValues(request);
        db.Entry(forecast).State = EntityState.Modified;

        try
        {
            await db.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            return StatusCode(500, exception);
        }

        var response = db.WeatherForecasts.Where(e => e.Id == id)
                                                .Select((forecast) => new WeatherForecastResponse
                                                {
                                                    Id = forecast.Id,
                                                    Date = forecast.Date,
                                                    TemperatureC = forecast.TemperatureC,
                                                    Summary = forecast.Summary
                                                });

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveWeatherForecast([FromRoute] Guid id)
    {
        var user = await userManager.GetUserAsync(HttpContext.User);
        if (user is null)
        {
            return Unauthorized();
        }

        var forecast = await db.WeatherForecasts.Where(e => e.UserId == user.Id)
                                                      .Where(e => e.Id == id)
                                                      .SingleOrDefaultAsync();
        if (forecast is null)
        {
            return NotFound();
        }

        db.WeatherForecasts.Remove(forecast);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            return StatusCode(500, exception);
        }

        return NoContent();
    }

    [HttpPost("bulk-delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveWeatherForecasts([FromBody] List<Guid> ids)
    {
        var courses = await db.WeatherForecasts
            .Where(c => ids.Contains(c.Id))
            .ToListAsync();

        if (courses.Count == 0)
        {
            return NotFound();
        }

        db.WeatherForecasts.RemoveRange(courses);
        await db.SaveChangesAsync();

        return NoContent();
    }
}

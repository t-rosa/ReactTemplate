using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactTemplate.Server.Database;
using ReactTemplate.Server.Modules.Auth;
using ReactTemplate.Server.Modules.Users;
using ReactTemplate.Server.Modules.WeatherForecasts.DTOs;

namespace ReactTemplate.Server.Modules.WeatherForecasts;

[Authorize(Roles = $"{Roles.Member}, {Roles.Admin}")]
[ApiController]
[Route("api/weather-forecasts")]
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

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(WeatherForecastResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWeatherForecast([FromRoute] string id)
    {
        var user = await userManager.GetUserAsync(HttpContext.User);
        if (user is null)
        {
            return Unauthorized();
        }

        var query = db.WeatherForecasts
            .Where(e => e.UserId == user.Id)
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
        await validator.ValidateAndThrowAsync(request);

        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        WeatherForecast weatherForecast = request.ToEntity(user.Id);

        db.WeatherForecasts.Add(weatherForecast);

        await db.SaveChangesAsync();

        var response = weatherForecast.ToWeatherForecastResponse();

        return CreatedAtAction(nameof(GetWeatherForecast), new { id = response.Id }, response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateWeatherForecast(
        [FromRoute] string id,
        [FromBody] UpdateWeatherForecastRequest request,
        [FromServices] IValidator<UpdateWeatherForecastRequest> validator)
    {
        await validator.ValidateAndThrowAsync(request);

        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var forecast = await db.WeatherForecasts
            .Where(e => e.UserId == user.Id)
            .Where(e => e.Id == id)
            .SingleOrDefaultAsync();

        if (forecast is null)
        {
            return NotFound();
        }

        forecast.UpdateFromRequest(request, user.Id);

        await db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveWeatherForecast([FromRoute] string id)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var forecast = await db.WeatherForecasts
            .Where(e => e.UserId == user.Id)
            .Where(e => e.Id == id)
            .SingleOrDefaultAsync();

        if (forecast is null)
        {
            return NotFound();
        }

        db.WeatherForecasts.Remove(forecast);

        await db.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("bulk-delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveWeatherForecasts([FromBody] List<string> ids)
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

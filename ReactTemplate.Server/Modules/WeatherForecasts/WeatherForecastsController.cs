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
public class WeatherForecastsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public WeatherForecastsController(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<GetWeatherForecastResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWeatherForecasts()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user is null)
        {
            return Unauthorized();
        }


        var response = _context.WeatherForecasts.AsNoTracking()
                                                .Where(e => e.UserId == user.Id)
                                                .Select((forecast) => new GetWeatherForecastResponse(forecast.Id, forecast.Date, forecast.TemperatureC, forecast.Summary));
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetWeatherForecastResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWeatherForecast([FromRoute] Guid id)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user is null)
        {
            return Unauthorized();
        }

        var query = _context.WeatherForecasts.Where(e => e.UserId == user.Id)
                                             .Where(e => e.Id == id)
                                             .Select((forecast) => new GetWeatherForecastResponse(forecast.Id, forecast.Date, forecast.TemperatureC, forecast.Summary));

        var response = await query.SingleOrDefaultAsync();
        if (response is null)
        {
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
        var user = await _userManager.GetUserAsync(HttpContext.User);
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

        await _context.WeatherForecasts.AddAsync(forecast);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            return StatusCode(500, exception);
        }

        var response = new GetWeatherForecastResponse(forecast.Id, forecast.Date, forecast.TemperatureC, forecast.Summary);

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
        var user = await _userManager.GetUserAsync(HttpContext.User);
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

        var forecast = await _context.WeatherForecasts.Where(e => e.UserId == user.Id)
                                                      .Where(e => e.Id == id)
                                                      .SingleOrDefaultAsync();
        if (forecast is null)
        {
            return NotFound();
        }

        _context.Entry(forecast).CurrentValues.SetValues(request);
        _context.Entry(forecast).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            return StatusCode(500, exception);
        }

        var response = _context.WeatherForecasts.Where(e => e.Id == id)
                                                .Select((forecast) => new GetWeatherForecastResponse(forecast.Id, forecast.Date, forecast.TemperatureC, forecast.Summary));

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveWeatherForecast([FromRoute] Guid id)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user is null)
        {
            return Unauthorized();
        }

        var forecast = await _context.WeatherForecasts.Where(e => e.UserId == user.Id)
                                                      .Where(e => e.Id == id)
                                                      .SingleOrDefaultAsync();
        if (forecast is null)
        {
            return NotFound();
        }

        _context.WeatherForecasts.Remove(forecast);

        try
        {
            await _context.SaveChangesAsync();
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
        var courses = await _context.WeatherForecasts
            .Where(c => ids.Contains(c.Id))
            .ToListAsync();

        if (courses.Count == 0)
        {
            return NotFound();
        }

        _context.WeatherForecasts.RemoveRange(courses);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

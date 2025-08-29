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
    private readonly ILogger<WeatherForecastsController> _logger;
    private readonly UserManager<User> _userManager;

    public WeatherForecastsController(ApplicationDbContext context, ILogger<WeatherForecastsController> logger, UserManager<User> userManager)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
    }

    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<GetWeatherForecastResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWeatherForecasts()
    {
        _logger.LogInformation("Retrieving all weather forecasts");

        _logger.LogInformation("Retrieving the user");
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user is null)
        {
            _logger.LogError("User not found");
            return Unauthorized();
        }

        var response = _context.WeatherForecasts
            .AsNoTracking()
            .Where(e => e.UserId == user.Id)
            .Select((forecast) => new GetWeatherForecastResponse
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
        _logger.LogInformation("Retrieving weather forecasts with ID: {ID}", id);

        _logger.LogInformation("Retrieving the user");
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user is null)
        {
            _logger.LogError("User not found");
            return Unauthorized();
        }

        var query = _context.WeatherForecasts.Where(e => e.UserId == user.Id).Where(e => e.Id == id).Select((forecast) => new GetWeatherForecastResponse
        {
            Id = forecast.Id,
            Date = forecast.Date,
            Summary = forecast.Summary,
            TemperatureC = forecast.TemperatureC
        });

        var response = await query.SingleOrDefaultAsync();
        if (response is null)
        {
            _logger.LogInformation("Weather forecasts with ID: {ID} not found", id);
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
        _logger.LogInformation("Creating weather forecast");

        _logger.LogInformation("Retrieving the user");
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user is null)
        {
            _logger.LogError("User not found");
            return Unauthorized();
        }

        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            _logger.LogError("Error validating request");
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
        _logger.LogInformation("Creating weather forecast with ID: {ID}", forecast.Id);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError("Error creating weather forecast with ID: {ID}", forecast.Id);
            return StatusCode(500, exception);
        }

        var response = new GetWeatherForecastResponse
        {
            Id = forecast.Id,
            Date = forecast.Date,
            Summary = forecast.Summary,
            TemperatureC = forecast.TemperatureC,
        };

        _logger.LogInformation("Weather forecast with ID: {ID} successfully created", response.Id);
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
        _logger.LogInformation("Updating weather forecast with ID: {ID}", id);

        _logger.LogInformation("Retrieving the user");
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user is null)
        {
            _logger.LogError("User not found");
            return Unauthorized();
        }

        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            _logger.LogError("Error validating request");
            var problem = new ValidationProblemDetails(validation.ToDictionary());
            return ValidationProblem(problem);
        }

        var forecast = await _context.WeatherForecasts.Where(e => e.UserId == user.Id).Where(e => e.Id == id).SingleOrDefaultAsync();
        if (forecast is null)
        {
            _logger.LogInformation("Weather forecasts with ID: {ID} not found", id);
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
            _logger.LogError("Error updating weather forecast with ID: {ID}", id);
            return StatusCode(500, exception);
        }

        _logger.LogInformation("Weather forecast with ID: {ID} successfully updated", id);

        var response = _context.WeatherForecasts.Where(e => e.Id == id).Select((forecast) => new GetWeatherForecastResponse
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
        _logger.LogInformation("Removing weather forecast with ID: {ID}", id);

        _logger.LogInformation("Retrieving the user");
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user is null)
        {
            _logger.LogError("User not found");
            return Unauthorized();
        }

        var forecast = await _context.WeatherForecasts.Where(e => e.UserId == user.Id).Where(e => e.Id == id).SingleOrDefaultAsync();
        if (forecast is null)
        {
            _logger.LogInformation("Weather forecasts with ID: {ID} not found", id);
            return NotFound();
        }

        _context.WeatherForecasts.Remove(forecast);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError("Error removing weather forecast with ID: {ID}", id);
            return StatusCode(500, exception);
        }

        _logger.LogInformation("Weather forecast with ID: {ID} successfully removed", id);
        return NoContent();
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactTemplate.WeatherForecast.Dtos;

namespace ReactTemplate.WeatherForecast;

[ApiController]
[Route("api/weather-forecast")]
public class WeatherForecastController(WeatherForecastDbContext context) : ControllerBase
{
    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<GetWeatherForecast>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetWeatherForecasts()
    {
        var response = context.WeatherForecasts.Select((forecast) => new GetWeatherForecast
        {
            Date = forecast.Date,
            Summary = forecast.Summary,
            TemperatureC = forecast.TemperatureC
        });

        return Ok(response);
    }
}

using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ReactTemplate.WeatherForecast.Dtos;

namespace ReactTemplate.WeatherForecasts.Tests;

public class WeatherForecastControllerTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetWeatherForecasts_ReturnUnauthorized()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/api/weather-forecast");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnUnauthorized()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync($"/api/weather-forecast/{Guid.Parse("739173e5-58b8-48fa-a44e-b7985baef3b6")}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateWeatherForecast_ReturnUnauthorized()
    {
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/weather-forecast", new CreateWeatherForecastRequest
        {
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            TemperatureC = 30
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdateWeatherForecast_ReturnUnauthorized()
    {
        var client = factory.CreateClient();

        var response = await client.PutAsJsonAsync($"/api/weather-forecast/{Guid.Parse("739173e5-58b8-48fa-a44e-b7985baef3b6")}", new UpdateWeatherForecastRequest
        {
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            TemperatureC = 30
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RemoveWeatherForecast_ReturnUnauthorized()
    {
        var client = factory.CreateClient();

        var response = await client.DeleteAsync($"/api/weather-forecast/{Guid.Parse("739173e5-58b8-48fa-a44e-b7985baef3b6")}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}

using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using ReactTemplate.Server.Modules.WeatherForecasts.Dtos;

namespace ReactTemplate.Tests.Modules.WeatherForecasts;

public class WeatherForecastControllerTests(BaseFactory factory) : IClassFixture<BaseFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private readonly Faker<CreateWeatherForecastRequest> _createWeatherForecastFaker = new Faker<CreateWeatherForecastRequest>()
        .RuleFor(x => x.Date, faker => faker.Date.RecentDateOnly())
        .RuleFor(x => x.TemperatureC, faker => faker.Random.Int())
        .RuleFor(x => x.Summary, faker => faker.Lorem.Paragraph());

    [Fact]
    public async Task GetWeatherForecasts_ReturnUnauthorized()
    {
        var response = await _client.GetAsync("/api/weather-forecast");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData("739173e5-58b8-48fa-a44e-b7985baef3b6")]
    public async Task GetWeatherForecast_ReturnUnauthorized(string id)
    {
        var response = await _client.GetAsync($"/api/weather-forecast/{Guid.Parse(id)}");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateWeatherForecast_ReturnUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("/api/weather-forecast", _createWeatherForecastFaker.Generate());

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData("739173e5-58b8-48fa-a44e-b7985baef3b6")]
    public async Task UpdateWeatherForecast_ReturnUnauthorized(string id)
    {
        var response = await _client.PutAsJsonAsync($"/api/weather-forecast/{Guid.Parse(id)}", _createWeatherForecastFaker.Generate());

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData("739173e5-58b8-48fa-a44e-b7985baef3b6")]
    public async Task RemoveWeatherForecast_ReturnUnauthorized(string id)
    {
        var response = await _client.DeleteAsync($"/api/weather-forecast/{Guid.Parse(id)}");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }


}

using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReactTemplate.Server.Modules.WeatherForecasts.Dtos;

namespace ReactTemplate.Tests.Modules.WeatherForecasts;

[Collection("DefaultCollection")]
public class WeatherForecastsControllerTests : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    private readonly Faker<CreateWeatherForecastRequest> _createWeatherForecastFaker = new Faker<CreateWeatherForecastRequest>()
        .RuleFor(x => x.Date, faker => faker.Date.RecentDateOnly())
        .RuleFor(x => x.TemperatureC, faker => faker.Random.Int(-30, 50))
        .RuleFor(x => x.Summary, faker => faker.Lorem.Sentence());

    private readonly Faker<UpdateWeatherForecastRequest> _updateWeatherForecastFaker = new Faker<UpdateWeatherForecastRequest>()
        .RuleFor(x => x.Date, faker => faker.Date.SoonDateOnly())
        .RuleFor(x => x.TemperatureC, faker => faker.Random.Int(-30, 50))
        .RuleFor(x => x.Summary, faker => faker.Lorem.Sentence());

    public WeatherForecastsControllerTests(BaseFactory factory)
    {
        _client = factory.CreateClient();

        var scope = factory.Services.CreateScope();
        _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    }

    public async Task InitializeAsync()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login?useCookies=true", new
        {
            email = _configuration["ADMIN_EMAIL"],
            password = _configuration["ADMIN_PASSWORD"]
        });

        response.EnsureSuccessStatusCode();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetWeatherForecasts_ReturnOK()
    {
        var response = await _client.GetAsync("/api/weather-forecasts");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var forecasts = await response.Content.ReadFromJsonAsync<IEnumerable<GetWeatherForecastResponse>>();
        forecasts.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateWeatherForecast_ReturnCreated()
    {
        var request = _createWeatherForecastFaker.Generate();

        var response = await _client.PostAsJsonAsync("/api/weather-forecasts", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await response.Content.ReadFromJsonAsync<GetWeatherForecastResponse>();
        created.Should().NotBeNull();
        created.Summary.Should().Be(request.Summary);
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnOK()
    {
        var request = _createWeatherForecastFaker.Generate();
        var postResponse = await _client.PostAsJsonAsync("/api/weather-forecasts", request);
        var created = await postResponse.Content.ReadFromJsonAsync<GetWeatherForecastResponse>();

        var getResponse = await _client.GetAsync($"/api/weather-forecasts/{created!.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var fetched = await getResponse.Content.ReadFromJsonAsync<GetWeatherForecastResponse>();
        fetched.Should().NotBeNull();
        fetched.Id.Should().Be(created.Id);
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnNotFound()
    {
        var response = await _client.GetAsync($"/api/weather-forecasts/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateWeatherForecast_ReturnOK()
    {
        var createRequest = _createWeatherForecastFaker.Generate();
        var createResponse = await _client.PostAsJsonAsync("/api/weather-forecasts", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<GetWeatherForecastResponse>();

        var updateRequest = _updateWeatherForecastFaker.Generate();

        var updateResponse = await _client.PutAsJsonAsync($"/api/weather-forecasts/{created!.Id}", updateRequest);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateWeatherForecast_ReturnNotFound()
    {
        var updateRequest = _updateWeatherForecastFaker.Generate();
        var response = await _client.PutAsJsonAsync($"/api/weather-forecasts/{Guid.NewGuid()}", updateRequest);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteWeatherForecast_ReturnNoContent()
    {
        var request = _createWeatherForecastFaker.Generate();
        var postResponse = await _client.PostAsJsonAsync("/api/weather-forecasts", request);
        var created = await postResponse.Content.ReadFromJsonAsync<GetWeatherForecastResponse>();

        var deleteResponse = await _client.DeleteAsync($"/api/weather-forecasts/{created!.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/weather-forecasts/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteWeatherForecast_ReturnNotFound()
    {
        var response = await _client.DeleteAsync($"/api/weather-forecasts/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

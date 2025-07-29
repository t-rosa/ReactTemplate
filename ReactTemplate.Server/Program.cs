using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ReactTemplate.Authentication;
using ReactTemplate.Server.Services;
using ReactTemplate.WeatherForecasts;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Module Services
builder.Services.AddWeatherForecastServices(builder.Configuration);
builder.Services.AddAuthenticationServices(builder.Configuration, builder.Environment);

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("SmtpOptions"));

builder.Services.AddProblemDetails();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.ApplyAuthenticationMigrations();
    app.ApplyWeatherForecastMigrations();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapIdentityApi<IdentityUser>();

app.MapControllers();

app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager,
    [FromBody] object empty) =>
{
    if (empty != null)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
    return Results.Unauthorized();
})
.WithOpenApi()
.RequireAuthorization();

app.MapFallbackToFile("/index.html");

app.Run();

public partial class Program { } // needed for tests

using ReactTemplate.Server.Extensions;
using Scalar.AspNetCore;

namespace ReactTemplate.Server;

public interface IProgram
{
    private static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder
            .AddControllers()
            .AddDatabase()
            .AddErrorHandling()
            .AddObservability()
            .AddAuthenticationServices()
            .AddEmailServices()
            .AddApplicationServices();

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();

            await app.ApplyMigrationAsync();
            await app.SeedInitialDataAsync();
        }

        app.UseResponseCompression();

        app.UseHttpsRedirection();

        app.UseExceptionHandler();

        app.UseAuthorization();

        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        await app.RunAsync();
    }
}

using ReactTemplate.Server.Extensions;
using Scalar.AspNetCore;

namespace ReactTemplate.Server;

public interface IProgram
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder
            .AddControllers()
            .AddDatabase()
            .AddObservability()
            .AddAuthenticationServices()
            .AddApplicationServices();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();

            await app.ApplyMigrationAsync();
        }

        await app.SeedRolesAsync();
        await app.SeedAdminUserAsync();

        if (app.Environment.IsDevelopment())
        {
            await app.SeedTestUsersAsync();
        }

        app.UseResponseCompression();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        await app.RunAsync();
    }
}

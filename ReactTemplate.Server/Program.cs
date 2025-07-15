using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactTemplate.Server;
using ReactTemplate.Server.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("ReactTemplate"))
        .UseSnakeCaseNamingConvention());

builder.Services.AddAuthorization();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo("./keys/storage"));
}
else
{
    builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo("keys/storage"));
}

builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
    options.User.RequireUniqueEmail = true;
});

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

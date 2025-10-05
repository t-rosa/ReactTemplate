using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReactTemplate.Server.Modules.Users;
using ReactTemplate.Server.Modules.WeatherForecasts;

namespace ReactTemplate.Server;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    internal DbSet<WeatherForecast> WeatherForecasts { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WeatherForecast>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                  .ValueGeneratedOnAdd();

            entity.HasOne(e => e.User)
                  .WithMany(u => u.WeatherForecasts)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .IsRequired();

            entity.Property(e => e.Date)
                  .IsRequired()
                  .HasColumnType("date");

            entity.Property(e => e.TemperatureC)
                  .IsRequired();

            entity.Property(e => e.Summary)
                  .HasMaxLength(200)
                  .IsRequired(false);

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Date);
            entity.HasIndex(e => new { e.UserId, e.Date });
        });
    }
}

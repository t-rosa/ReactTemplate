using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ReactTemplate.Server.Modules.WeatherForecasts;

public sealed class WeatherForecastConfiguration : IEntityTypeConfiguration<WeatherForecast>
{
    public void Configure(EntityTypeBuilder<WeatherForecast> builder)
    {
        builder.HasOne(e => e.User)
              .WithMany(u => u.WeatherForecasts)
              .HasForeignKey(e => e.UserId)
              .OnDelete(DeleteBehavior.Cascade)
              .IsRequired();

        builder.Property(e => e.Date)
              .IsRequired()
              .HasColumnType("date");

        builder.Property(e => e.TemperatureC)
              .IsRequired();

        builder.Property(e => e.Summary)
              .HasMaxLength(200)
              .IsRequired(false);

        builder.Property(e => e.CreatedAt)
              .IsRequired()
              .HasColumnType("timestamp with time zone");

        builder.Property(e => e.UpdatedAt)
              .HasColumnType("timestamp with time zone")
              .IsRequired(false);
    }
}

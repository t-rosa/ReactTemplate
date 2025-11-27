using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReactWeaver.Server.Modules.Users;

namespace ReactWeaver.Server.Modules.WeatherForecasts;

public sealed class WeatherForecastConfiguration : IEntityTypeConfiguration<WeatherForecast>
{
      public void Configure(EntityTypeBuilder<WeatherForecast> builder)
      {
            builder.HasKey(wf => wf.Id);

            builder
                  .Property(wf => wf.Id)
                  .HasMaxLength(500);

            builder
                  .Property(wf => wf.UserId)
                  .HasMaxLength(500);

            builder
                  .Property(wf => wf.Date);

            builder
                  .Property(wf => wf.TemperatureC);

            builder
                  .Property(wf => wf.Summary)
                  .HasMaxLength(100);

            builder
                  .HasOne<User>()
                  .WithMany()
                  .HasForeignKey(wf => wf.UserId);
      }
}

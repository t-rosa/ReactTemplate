using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReactTemplate.Server.Modules.Common;
using ReactTemplate.Server.Modules.Users;
using ReactTemplate.Server.Modules.WeatherForecasts;

namespace ReactTemplate.Server;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    internal DbSet<WeatherForecast> WeatherForecasts { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        HandleAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void HandleAuditFields()
    {
        var currentUserId = GetCurrentUserId();
        var now = DateTime.UtcNow;

        var entries = ChangeTracker.Entries<IAuditableEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = currentUserId;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = currentUserId;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = now;
                    entry.Entity.DeletedBy = currentUserId;
                    break;
            }
        }
    }

    private Guid? GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor?.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim != null && Guid.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }

        return null;
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

            // Configuration des propriétés d'audit
            entity.Property(e => e.CreatedAt)
                  .IsRequired()
                  .HasColumnType("timestamp with time zone");

            entity.Property(e => e.CreatedBy)
                  .IsRequired(false);

            entity.Property(e => e.UpdatedAt)
                  .HasColumnType("timestamp with time zone")
                  .IsRequired(false);

            entity.Property(e => e.UpdatedBy)
                  .IsRequired(false);

            // Configuration des propriétés de soft delete
            entity.Property(e => e.IsDeleted)
                  .IsRequired()
                  .HasDefaultValue(false);

            entity.Property(e => e.DeletedAt)
                  .HasColumnType("timestamp with time zone")
                  .IsRequired(false);

            entity.Property(e => e.DeletedBy)
                  .IsRequired(false);

            // Global Query Filter: Automatically exclude deleted entities (soft delete)
            // Entities with IsDeleted = true will not be returned by default
            // To explicitly include them: query.IgnoreQueryFilters()
            entity.HasQueryFilter(e => !e.IsDeleted);

            // Optional relations to User for audit
            // Note: These relations are NOT configured as they would create cycles
            // and complicate deletion. We keep only the Guid without FK.
            // To retrieve user info, perform a manual join if necessary.

            // Index
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Date);
            entity.HasIndex(e => new { e.UserId, e.Date });
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.CreatedBy);
            entity.HasIndex(e => e.UpdatedBy);
            entity.HasIndex(e => e.IsDeleted);
            entity.HasIndex(e => e.DeletedAt);
        });
    }
}

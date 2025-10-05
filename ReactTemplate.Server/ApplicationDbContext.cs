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
        // Remplir automatiquement les champs d'audit avant de sauvegarder
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

            // Relations optionnelles vers User pour l'audit
            // Note: Ces relations ne sont PAS configurées car elles créeraient des cycles
            // et compliqueraient la suppression. On garde juste les Guid sans FK.
            // Pour récupérer les infos utilisateur, faire une jointure manuelle si nécessaire.

            // Index
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Date);
            entity.HasIndex(e => new { e.UserId, e.Date });
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.CreatedBy);
            entity.HasIndex(e => e.UpdatedBy);
        });
    }
}

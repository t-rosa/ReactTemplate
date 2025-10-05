# Audit Trail - Tracking des Modifications

## 🎯 Objectif

Le système d'audit trail permet de tracer automatiquement :

- **Qui** a créé une entité
- **Quand** elle a été créée
- **Qui** l'a modifiée en dernier
- **Quand** elle a été modifiée en dernier

## 📋 Implémentation

### 1. Interface `IAuditableEntity`

```csharp
public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }
    Guid? CreatedBy { get; set; }        // ✅ Guid de l'utilisateur
    DateTime? UpdatedAt { get; set; }
    Guid? UpdatedBy { get; set; }        // ✅ Guid de l'utilisateur
}
```

### 2. Classe de Base `AuditableEntity`

```csharp
public abstract class AuditableEntity : IAuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }     // ✅ ID utilisateur (immuable)
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }     // ✅ ID utilisateur (immuable)
}
```

### 3. Utilisation dans les Entités

```csharp
public class WeatherForecast : AuditableEntity
{
    public Guid Id { get; set; }
    // ... autres propriétés
}
```

### 4. Remplissage Automatique dans `ApplicationDbContext`

```csharp
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
        }
    }
}

private Guid? GetCurrentUserId()
{
    var userIdClaim = _httpContextAccessor?.HttpContext?.User
        ?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    if (userIdClaim != null && Guid.TryParse(userIdClaim, out var userId))
    {
        return userId;
    }

    return null;
}
```

## ✅ Avantages

### 1. **Automatique**

- Pas besoin de penser à remplir les champs manuellement
- Réduit les erreurs humaines
- Code plus propre

### 2. **Traçabilité Complète**

```csharp
var forecast = await context.WeatherForecasts
    .Include(f => f.User) // Pour afficher le créateur
    .FirstOrDefaultAsync(f => f.Id == id);

// Récupérer les infos des utilisateurs pour l'audit
var createdByUser = forecast.CreatedBy.HasValue
    ? await context.Users.FindAsync(forecast.CreatedBy.Value)
    : null;

Console.WriteLine($"Créé par {createdByUser?.Email} (ID: {forecast.CreatedBy}) le {forecast.CreatedAt}");
Console.WriteLine($"Modifié le {forecast.UpdatedAt}");
```

### 3. **Conformité & Réglementations**

- RGPD : Traçabilité des modifications de données personnelles
- SOX/ISO : Audit trail pour les données financières/sensibles
- Historique pour résolution de conflits

### 4. **Debug & Support**

```csharp
// Requête : Toutes les entités créées aujourd'hui
var todayEntities = await context.WeatherForecasts
    .Where(e => e.CreatedAt.Date == DateTime.UtcNow.Date)
    .ToListAsync();

// Requête : Dernières modifications par utilisateur
var userModifications = await context.WeatherForecasts
    .Where(e => e.UpdatedBy == userEmail)
    .OrderByDescending(e => e.UpdatedAt)
    .ToListAsync();
```

## 🔧 Configuration EF Core

### Properties Configuration

```csharp
// CreatedAt - Obligatoire
entity.Property(e => e.CreatedAt)
      .IsRequired()
      .HasColumnType("timestamp with time zone");

// CreatedBy - Optionnel (Guid de l'utilisateur)
entity.Property(e => e.CreatedBy)
      .IsRequired(false);

// UpdatedAt - Optionnel
entity.Property(e => e.UpdatedAt)
      .HasColumnType("timestamp with time zone")
      .IsRequired(false);

// UpdatedBy - Optionnel (Guid de l'utilisateur)
entity.Property(e => e.UpdatedBy)
      .IsRequired(false);

// Index pour les performances
entity.HasIndex(e => e.CreatedAt);
```

## 📊 Schéma Base de Données

```sql
-- Colonnes ajoutées à la table weather_forecasts
created_at        TIMESTAMP WITH TIME ZONE NOT NULL
created_by        UUID                     NULL  -- ✅ Guid de l'utilisateur
updated_at        TIMESTAMP WITH TIME ZONE NULL
updated_by        UUID                     NULL  -- ✅ Guid de l'utilisateur

-- Index
CREATE INDEX ix_weather_forecasts_created_at ON weather_forecasts(created_at);
CREATE INDEX ix_weather_forecasts_created_by ON weather_forecasts(created_by);
CREATE INDEX ix_weather_forecasts_updated_by ON weather_forecasts(updated_by);
```

## 🎓 Bonnes Pratiques

### ✅ À FAIRE

```csharp
// 1. Toujours utiliser DateTime.UtcNow
entry.Entity.CreatedAt = DateTime.UtcNow; // ✅

// 2. Stocker le Guid de l'utilisateur (immuable)
entry.Entity.CreatedBy = userId; // ✅ Guid

// 3. UpdatedAt et UpdatedBy sont nullables
public DateTime? UpdatedAt { get; set; } // ✅
public Guid? UpdatedBy { get; set; } // ✅
```

### ❌ À ÉVITER

```csharp
// ❌ Ne pas utiliser DateTime.Now (fuseau horaire local)
entry.Entity.CreatedAt = DateTime.Now;

// ❌ Ne pas stocker l'email ou le nom (peuvent changer)
entry.Entity.CreatedBy = user.Email; // L'email peut changer !
entry.Entity.CreatedBy = user.FullName; // Le nom peut changer !

// ❌ Ne pas rendre UpdatedAt obligatoire
public DateTime UpdatedAt { get; set; } // Pour une nouvelle entité, pas encore modifiée
```

## 🔑 Pourquoi Utiliser l'ID Utilisateur (Guid) ?

### ✅ Avantages du Guid

1. **Immuabilité** : L'ID utilisateur ne change jamais, contrairement à l'email
2. **Intégrité** : Peut servir de foreign key vers `AspNetUsers`
3. **RGPD** : Plus facile d'anonymiser (remplacer par un Guid fictif)
4. **Performance** : Guid (16 bytes) vs Email (jusqu'à 256 bytes)
5. **Normalisation** : Respect des principes de base de données relationnelle

### 📝 Récupération des Infos Utilisateur

```csharp
// Requête avec jointure pour afficher les emails
var forecasts = await context.WeatherForecasts
    .Select(f => new
    {
        f.Id,
        f.Date,
        f.CreatedAt,
        CreatedByEmail = context.Users
            .Where(u => u.Id == f.CreatedBy)
            .Select(u => u.Email)
            .FirstOrDefault()
    })
    .ToListAsync();
```

## 🚀 Extension Future : Soft Delete

Le pattern d'audit peut être étendu pour inclure le soft delete :

```csharp
public abstract class AuditableEntity : IAuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    // Soft Delete
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
}
```

## 📖 Utilisation dans les DTOs

Les champs d'audit peuvent être exposés dans les DTOs pour affichage :

```csharp
public record GetWeatherForecastResponse(
    Guid Id,
    DateOnly Date,
    int TemperatureC,
    string? Summary,
    DateTime CreatedAt,      // ✅ Exposé au client
    string? CreatedBy,       // ✅ Exposé au client
    DateTime? UpdatedAt,     // ✅ Exposé au client
    string? UpdatedBy        // ✅ Exposé au client
);
```

## 🔍 Requêtes Utiles

### Audit par Utilisateur

```csharp
var userId = Guid.Parse("...");
var userActivity = await context.WeatherForecasts
    .Where(e => e.CreatedBy == userId || e.UpdatedBy == userId)
    .OrderByDescending(e => e.UpdatedAt ?? e.CreatedAt)
    .ToListAsync();
```

### Entités Récemment Modifiées

```csharp
var recentChanges = await context.WeatherForecasts
    .Where(e => e.UpdatedAt.HasValue)
    .OrderByDescending(e => e.UpdatedAt)
    .Take(10)
    .ToListAsync();
```

### Statistiques

```csharp
var stats = await context.WeatherForecasts
    .GroupBy(e => e.CreatedBy)
    .Select(g => new
    {
        User = g.Key,
        Count = g.Count(),
        LastActivity = g.Max(e => e.UpdatedAt ?? e.CreatedAt)
    })
    .ToListAsync();
```

## 📚 Références

- [EF Core - Change Tracking](https://learn.microsoft.com/en-us/ef/core/change-tracking/)
- [Audit Entities in EF Core](https://learn.microsoft.com/en-us/ef/core/change-tracking/change-detection)
- [Temporal Tables (Alternative)](https://learn.microsoft.com/en-us/ef/core/providers/sql-server/temporal-tables)

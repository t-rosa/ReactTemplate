# Navigation Properties - Best Practices

## 🎯 Pattern Utilisé : Pattern Simple

Ce template utilise le **pattern simple** pour les propriétés de navigation, recommandé par Microsoft pour un bon équilibre entre simplicité et fonctionnalité.

## 📋 Implémentation

### Entité `User.cs`

```csharp
public class User : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    // Navigation property avec GETTER SEUL (best practice Microsoft)
    // ✅ Pas de setter - la collection ne peut pas être réassignée
    // ✅ Initialisation inline - pas de NullReferenceException
    public ICollection<WeatherForecast> WeatherForecasts { get; } = new List<WeatherForecast>();
}
```

### Entité `WeatherForecast.cs`

```csharp
public class WeatherForecast
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }

    // Navigation de référence (relation many-to-one)
    public User User { get; set; } = default!;
}
```

### Configuration EF Core `ApplicationDbContext.cs`

```csharp
// Configuration de la relation
modelBuilder.Entity<WeatherForecast>(entity =>
{
    entity.HasOne(e => e.User)
          .WithMany(u => u.WeatherForecasts)
          .HasForeignKey(e => e.UserId)
          .OnDelete(DeleteBehavior.Cascade);
});
```

## ✅ Avantages de ce Pattern (Getter Seul)

### 1. **Protection Contre la Réassignation**

```csharp
public ICollection<WeatherForecast> WeatherForecasts { get; } = new List<WeatherForecast>();

// ✅ Autorisé - Ajouter/supprimer des éléments
user.WeatherForecasts.Add(newForecast);

// ❌ Impossible - Réassigner toute la collection
user.WeatherForecasts = new List<WeatherForecast>(); // ERREUR DE COMPILATION
```

- Empêche de casser le change tracking d'EF Core
- Évite les bugs subtils liés au remplacement de collection

### 2. **Initialisation Automatique**

```csharp
{ get; } = new List<WeatherForecast>();
```

- Collection toujours initialisée, jamais `null`
- Pas besoin de vérifier `if (collection == null)`
- Évite les `NullReferenceException`

### 3. **Simplicité et Clarté**

- Code concis et explicite
- Intent clair : "Cette collection fait partie de l'entité"
- Pattern recommandé par Microsoft

### 4. **EF Core Friendly**

- EF Core peut charger, ajouter et supprimer des entités naturellement
- Fonctionne avec `Include()`, `ThenInclude()`, lazy loading, etc.
- Change tracking automatique
- **Important** : EF Core remplace l'instance lors du chargement si nécessaire

### 5. **Performance avec HashSet**

Pour les grandes collections, on peut utiliser `HashSet<T>` :

```csharp
public ICollection<WeatherForecast> WeatherForecasts { get; }
    = new HashSet<WeatherForecast>(ReferenceEqualityComparer.Instance);
```

- ✅ Recherches O(1) au lieu de O(n)
- ✅ `ReferenceEqualityComparer.Instance` pour l'égalité de référence (requis pour EF Core)

## ⚠️ Pièges à Éviter

### ❌ NE PAS FAIRE : Propriété d'expression

```csharp
// ❌ ERREUR - Crée une nouvelle liste à CHAQUE accès !
public ICollection<WeatherForecast> WeatherForecasts => new List<WeatherForecast>();
```

**Problème** : Chaque fois que vous accédez à la propriété, une nouvelle liste vide est créée.
EF Core ne pourra jamais charger les données !

### ❌ NE PAS FAIRE : Tableaux

```csharp
// ❌ ERREUR - Les tableaux ne peuvent pas être utilisés
public WeatherForecast[] WeatherForecasts { get; set; }
```

**Problème** : La méthode `Add()` des tableaux lève une exception. EF Core a besoin de `Add()`.

### ❌ NE PAS FAIRE : HashSet sans ReferenceEqualityComparer

```csharp
// ❌ ERREUR - Peut causer des problèmes de tracking
public ICollection<WeatherForecast> WeatherForecasts { get; } = new HashSet<WeatherForecast>();
```

**Solution** : Toujours utiliser `ReferenceEqualityComparer` avec HashSet :

```csharp
// ✅ CORRECT
public ICollection<WeatherForecast> WeatherForecasts { get; }
    = new HashSet<WeatherForecast>(ReferenceEqualityComparer.Instance);
```

### ✅ À FAIRE : Getter seul avec initialisation

```csharp
// ✅ PARFAIT - Pattern recommandé
public ICollection<WeatherForecast> WeatherForecasts { get; } = new List<WeatherForecast>();
```

## 🔧 Comment Utiliser Ce Pattern

### Chargement avec navigation :

```csharp
var user = await context.Users
    .Include(u => u.WeatherForecasts) // ✅ Charge la collection
    .FirstAsync();

// La collection est automatiquement peuplée par EF Core
Console.WriteLine($"Forecasts count: {user.WeatherForecasts.Count}");
```

### Ajout/Suppression direct :

```csharp
var user = await context.Users.FindAsync(userId);

// ✅ Ajout direct
user.WeatherForecasts.Add(new WeatherForecast
{
    Date = DateOnly.FromDateTime(DateTime.UtcNow),
    TemperatureC = 25
});

// ✅ Suppression direct
var forecast = user.WeatherForecasts.First();
user.WeatherForecasts.Remove(forecast);

await context.SaveChangesAsync();
```

### Requêtes avec navigation :

```csharp
// ✅ Filtrage par collection
var activeUsers = await context.Users
    .Where(u => u.WeatherForecasts.Any())
    .ToListAsync();

// ✅ Projection
var userStats = await context.Users
    .Select(u => new
    {
        u.Email,
        ForecastCount = u.WeatherForecasts.Count
    })
    .ToListAsync();
```

## 📚 Patterns Alternatifs (selon la doc Microsoft)

### ✅ Pattern Recommandé (Celui Utilisé)

```csharp
public ICollection<WeatherForecast> WeatherForecasts { get; } = new List<WeatherForecast>();
```

✅ Getter seul - pas de réassignation possible
✅ Simple et performant
✅ Recommandé par Microsoft

### ⚠️ Pattern avec Setter (À Éviter)

```csharp
public ICollection<WeatherForecast> WeatherForecasts { get; set; } = new List<WeatherForecast>();
```

✅ Simple
❌ Peut être réassigné (casse le change tracking EF Core)
❌ Moins sûr

### 📖 Pattern en Lecture Seule (IEnumerable)

```csharp
public IEnumerable<WeatherForecast> WeatherForecasts { get; } = new List<WeatherForecast>();
```

✅ Signal d'intention "lecture seule"
⚠️ Peut être casté en `ICollection<T>` et modifié
✅ Bon pour exposer des collections en DTO/API

### 🔒 Pattern Encapsulation Stricte (DDD)

```csharp
private readonly List<WeatherForecast> _weatherForecasts = new();
public IReadOnlyCollection<WeatherForecast> WeatherForecasts => _weatherForecasts;
public void AddWeatherForecast(WeatherForecast forecast) { /* logique métier */ }
```

✅ Contrôle total
✅ Logique métier dans l'entité
❌ Plus complexe
❌ Nécessite configuration EF Core (UsePropertyAccessMode)

## 🎓 Recommandations

### ✅ Quand Utiliser le Pattern Simple (celui-ci)

- **Templates et projets de départ** - Simplicité avant tout
- **Applications CRUD standards** - Pas de logique métier complexe
- **Petites à moyennes équipes** - Code facile à comprendre
- **Prototypes et MVP** - Rapidité de développement

### 🔄 Quand Considérer des Patterns Plus Complexes

Si votre application évolue et que vous avez besoin de :

- **Validation métier complexe** lors de l'ajout/suppression
- **Domain-Driven Design strict** avec agrégats
- **Invariants métier** à garantir sur les collections
- **Audit trail détaillé** de toutes les modifications

Alors vous pouvez évoluer vers le pattern d'encapsulation stricte :

```csharp
private readonly List<WeatherForecast> _weatherForecasts = new();
public IReadOnlyCollection<WeatherForecast> WeatherForecasts => _weatherForecasts;
public void AddWeatherForecast(WeatherForecast forecast) { /* logique métier */ }
```

## 📖 Références

- [Microsoft Docs - Relationship Navigations](https://learn.microsoft.com/en-us/ef/core/modeling/relationships/navigations)
- [EF Core - Property Access Mode](https://learn.microsoft.com/en-us/ef/core/modeling/backing-field)
- [DDD with EF Core](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/)

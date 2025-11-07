using System.Text.RegularExpressions;
using Spectre.Console;

namespace ReactTemplate.Tools.Services;

/// <summary>
/// Service to modify ApplicationDbContext.cs to add DbSet and OnModelCreating entries
/// </summary>
public class DbContextModifier
{
    private readonly string _dbContextPath;

    public DbContextModifier(string repoRoot)
    {
        _dbContextPath = Path.Combine(repoRoot, "ReactTemplate.Server", "ApplicationDbContext.cs");
    }

    /// <summary>
    /// Add a DbSet property to the DbContext if not already present
    /// </summary>
    public async Task<bool> AddDbSetAsync(string entityName, string moduleName)
    {
        if (!File.Exists(_dbContextPath))
        {
            AnsiConsole.MarkupLine($"[yellow]⚠ DbContext not found: {_dbContextPath}[/]");
            return false;
        }

        try
        {
            var content = await File.ReadAllTextAsync(_dbContextPath);

            // Check if DbSet already exists
            var dbSetPattern = $@"public\s+DbSet<{entityName}>\s+{GetPluralName(entityName)}\s*{{\s*get;\s*set;\s*}}";
            if (Regex.IsMatch(content, dbSetPattern))
            {
                AnsiConsole.MarkupLine($"[yellow]⚠ DbSet<{entityName}> already exists in DbContext[/]");
                return false;
            }

            // Find the position to insert the new DbSet (before closing brace of class)
            var lastDbSetMatch = Regex.Matches(content, @"public\s+DbSet<\w+>\s+\w+\s*{\s*get;\s*set;\s*}").Cast<Match>().LastOrDefault();

            if (lastDbSetMatch == null)
            {
                AnsiConsole.MarkupLine("[yellow]⚠ Could not find existing DbSets in DbContext[/]");
                return false;
            }

            var insertPosition = lastDbSetMatch.Index + lastDbSetMatch.Length;
            var newDbSet = $@"
    public DbSet<{entityName}> {GetPluralName(entityName)} {{ get; set; }}";

            var newContent = content.Insert(insertPosition, newDbSet);

            // Write back
            await File.WriteAllTextAsync(_dbContextPath, newContent);
            AnsiConsole.MarkupLine($"[green]✓ Added DbSet<{entityName}> to ApplicationDbContext[/]");

            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]✗ Error modifying DbContext: {ex.Message}[/]");
            return false;
        }
    }

    /// <summary>
    /// Add OnModelCreating configuration (suggestion only - user should implement)
    /// </summary>
    public string GetOnModelCreatingTemplate(string entityName, string moduleName)
    {
        return $@"
// TODO: Add to OnModelCreating() in ApplicationDbContext
modelBuilder.Entity<{entityName}>(entity =>
{{
    entity.ToTable(""{GetPluralName(entityName)}"", ""{moduleName.ToLower()}"");
    entity.HasKey(e => e.Id);

    // Add your entity configuration here
    // Example:
    // entity.Property(e => e.Name)
    //     .IsRequired()
    //     .HasMaxLength(256);
}});
";
    }

    private static string GetPluralName(string entityName)
    {
        // Simple pluralization (can be enhanced)
        if (entityName.EndsWith("y"))
            return entityName[..^1] + "ies";
        if (entityName.EndsWith("s"))
            return entityName + "es";

        return entityName + "s";
    }
}

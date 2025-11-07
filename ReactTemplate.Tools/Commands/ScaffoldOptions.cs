namespace ReactTemplate.Tools.Commands;

/// <summary>
/// Options pour la génération d'entité
/// </summary>
public class ScaffoldOptions
{
    public required string Module { get; set; }
    public required string Entity { get; set; }
    public string? Fields { get; set; }
    public string? SpecFile { get; set; }
    public bool GenerateTests { get; set; } = true;
    public bool SkipMigration { get; set; }
    public bool DryRun { get; set; }
}

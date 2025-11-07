namespace ReactTemplate.Tools.Models;

/// <summary>
/// Représente une propriété d'entité
/// </summary>
public class EntityField
{
    public required string Name { get; set; }
    public required string Type { get; set; }
    public bool IsRequired { get; set; }
    public bool IsPrimaryKey { get; set; }
    public string? DefaultValue { get; set; }
    public string? Validation { get; set; }
}

/// <summary>
/// Schéma complet d'une entité
/// </summary>
public class EntitySchema
{
    public required string EntityName { get; set; }
    public required string ModuleName { get; set; }
    public List<EntityField> Fields { get; set; } = [];
    public bool HasSoftDelete { get; set; }
    public bool HasAuditFields { get; set; } = true;
    public List<string> Tags { get; set; } = [];
    public string? Description { get; set; }
}

using System.IO;
using ReactTemplate.Tools.Models;
using Scriban;

namespace ReactTemplate.Tools.Services;

/// <summary>
/// Service pour rendre les templates Scriban avec les données d'entité
/// </summary>
public class TemplateRenderer
{
    private readonly string _templatesPath;

    public TemplateRenderer()
    {
        // Les templates sont situés dans le répertoire Templates relative à l'exécutable
        var executablePath = AppContext.BaseDirectory;
        _templatesPath = Path.Combine(executablePath, "Templates");
    }

    /// <summary>
    /// Rend un template Scriban avec les données du schéma
    /// </summary>
    public string RenderTemplate(string templateName, EntitySchema schema)
    {
        var templatePath = Path.Combine(_templatesPath, $"{templateName}.scriban");

        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException($"Template not found: {templatePath}");
        }

        var templateContent = File.ReadAllText(templatePath);
        var template = Template.Parse(templateContent);

        var data = BuildTemplateData(schema);
        return template.Render(data);
    }

    private object BuildTemplateData(EntitySchema schema)
    {
        return new
        {
            entity_name = schema.EntityName,
            entity_name_lower = schema.EntityName.ToLowerInvariant(),
            entity_name_camelcase = ToCamelCase(schema.EntityName),
            entity_name_snakecase = ToSnakeCase(schema.EntityName),
            entity_slug = ToSnakeCase(schema.EntityName).Replace("_", "-"),
            module_name = schema.ModuleName,
            module_name_lower = schema.ModuleName.ToLowerInvariant(),
            has_soft_delete = schema.HasSoftDelete,
            has_audit_fields = schema.HasAuditFields,
            description = schema.Description,
            fields = schema.Fields.Select(f => new
            {
                name = f.Name,
                name_lower = f.Name.ToLowerInvariant(),
                name_camelcase = ToCamelCase(f.Name),
                type = f.Type,
                is_required = f.IsRequired,
                is_primary_key = f.IsPrimaryKey,
                default_value = f.DefaultValue,
                validation = f.Validation,
                cs_type = MapCsType(f.Type),
                ts_type = MapTsType(f.Type),
            }).ToList(),
            primary_key_field = schema.Fields.FirstOrDefault(f => f.IsPrimaryKey)?.Name ?? "Id"
        };
    }

    private string ToCamelCase(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;
        return char.ToLower(text[0]) + text.Substring(1);
    }

    private string ToSnakeCase(string text)
    {
        var result = string.Empty;
        foreach (var c in text)
        {
            if (char.IsUpper(c) && result.Length > 0)
                result += "_";
            result += char.ToLower(c);
        }
        return result;
    }

    private string MapCsType(string type) => type.ToLowerInvariant() switch
    {
        "string" => "string",
        "int" => "int",
        "long" => "long",
        "decimal" => "decimal",
        "bool" => "bool",
        "datetime" => "DateTime",
        "dateonly" => "DateOnly",
        "guid" => "Guid",
        "double" => "double",
        "float" => "float",
        _ => type
    };

    private string MapTsType(string type) => type.ToLowerInvariant() switch
    {
        "string" => "string",
        "int" or "long" => "number",
        "decimal" or "double" or "float" => "number",
        "bool" => "boolean",
        "datetime" or "dateonly" => "Date | string",
        "guid" => "string",
        _ => "unknown"
    };
}

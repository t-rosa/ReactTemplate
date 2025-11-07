using System.Text.RegularExpressions;
using ReactTemplate.Tools.Models;

namespace ReactTemplate.Tools.Services;

/// <summary>
/// Service pour parser les définitions de champs et fichiers spec
/// </summary>
public class FieldParser
{
    /// <summary>
    /// Parse une string de champs au format "name:type required; active:bool; count:int"
    /// </summary>
    public List<EntityField> ParseFieldsString(string fieldsInput)
    {
        var fields = new List<EntityField>();
        var fieldDefinitions = fieldsInput.Split(';', StringSplitOptions.RemoveEmptyEntries);

        foreach (var definition in fieldDefinitions)
        {
            var field = ParseSingleField(definition.Trim());
            if (field != null)
            {
                fields.Add(field);
            }
        }

        return fields;
    }

    /// <summary>
    /// Parse une définition de champ simple: "name:string required" ou "active:bool"
    /// </summary>
    private EntityField? ParseSingleField(string definition)
    {
        if (string.IsNullOrWhiteSpace(definition))
            return null;

        var parts = definition.Split(new[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
            return null;

        var name = parts[0];
        var type = NormalizeType(parts[1]);

        var field = new EntityField
        {
            Name = CapitalizeFirstLetter(name),
            Type = type,
            IsRequired = definition.Contains("required", StringComparison.OrdinalIgnoreCase),
            IsPrimaryKey = definition.Contains("primary", StringComparison.OrdinalIgnoreCase),
            DefaultValue = ExtractDefaultValue(definition)
        };

        return field;
    }

    /// <summary>
    /// Normalise les types (e.g. string -> string, int -> int, bool -> bool)
    /// </summary>
    private string NormalizeType(string type) => type.ToLowerInvariant() switch
    {
        "str" or "text" => "string",
        "num" or "number" => "int",
        "bool" or "boolean" => "bool",
        "date" or "datetime" => "DateTime",
        "guid" => "Guid",
        "decimal" or "double" or "float" => "decimal",
        _ => type
    };

    /// <summary>
    /// Extrait la valeur par défaut si présente (e.g. "default=true")
    /// </summary>
    private string? ExtractDefaultValue(string definition)
    {
        var match = Regex.Match(definition, @"default=([^\s;]+)", RegexOptions.IgnoreCase);
        return match.Success ? match.Groups[1].Value : null;
    }

    private string CapitalizeFirstLetter(string text) =>
        string.IsNullOrEmpty(text) ? text : char.ToUpper(text[0]) + text.Substring(1);
}

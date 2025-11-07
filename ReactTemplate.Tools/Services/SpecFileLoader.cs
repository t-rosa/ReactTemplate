using System.IO;
using System.Text.Json;
using ReactTemplate.Tools.Models;
using YamlDotNet.Serialization;

namespace ReactTemplate.Tools.Services;

/// <summary>
/// Service pour charger les specs d'entités depuis des fichiers JSON/YAML
/// </summary>
public class SpecFileLoader
{
    /// <summary>
    /// Charge une spécification depuis un fichier JSON ou YAML
    /// </summary>
    public EntitySchema LoadSpec(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Spec file not found: {filePath}");
        }

        var content = File.ReadAllText(filePath);
        var extension = Path.GetExtension(filePath).ToLowerInvariant();

        return extension switch
        {
            ".json" => LoadJsonSpec(content),
            ".yaml" or ".yml" => LoadYamlSpec(content),
            _ => throw new InvalidOperationException($"Unsupported spec file format: {extension}")
        };
    }

    private EntitySchema LoadJsonSpec(string content)
    {
        using var doc = JsonDocument.Parse(content);
        var root = doc.RootElement;

        var fields = new List<EntityField>();
        var schema = new EntitySchema
        {
            EntityName = root.GetProperty("entityName").GetString() ?? throw new InvalidOperationException("Missing entityName"),
            ModuleName = root.GetProperty("moduleName").GetString() ?? throw new InvalidOperationException("Missing moduleName"),
            HasSoftDelete = root.TryGetProperty("hasSoftDelete", out var hasSoftDelete) && hasSoftDelete.GetBoolean(),
            HasAuditFields = !root.TryGetProperty("hasAuditFields", out var hasAudit) || hasAudit.GetBoolean(),
            Description = root.TryGetProperty("description", out var desc) ? desc.GetString() : null,
            Fields = fields
        };

        if (root.TryGetProperty("fields", out var fieldsElement))
        {
            foreach (var fieldElement in fieldsElement.EnumerateArray())
            {
                schema.Fields.Add(new EntityField
                {
                    Name = fieldElement.GetProperty("name").GetString() ?? "",
                    Type = fieldElement.GetProperty("type").GetString() ?? "string",
                    IsRequired = fieldElement.TryGetProperty("required", out var req) && req.GetBoolean(),
                    IsPrimaryKey = fieldElement.TryGetProperty("isPrimaryKey", out var pk) && pk.GetBoolean(),
                    DefaultValue = fieldElement.TryGetProperty("defaultValue", out var def) ? def.GetString() : null,
                    Validation = fieldElement.TryGetProperty("validation", out var val) ? val.GetString() : null,
                });
            }
        }

        if (root.TryGetProperty("tags", out var tagsElement))
        {
            foreach (var tag in tagsElement.EnumerateArray())
            {
                schema.Tags.Add(tag.GetString() ?? "");
            }
        }

        return schema;
    }

    private EntitySchema LoadYamlSpec(string content)
    {
        var deserializer = new DeserializerBuilder()
            .IgnoreUnmatchedProperties()
            .Build();

        var data = deserializer.Deserialize<Dictionary<object, object>>(content);
        if (data == null)
            throw new InvalidOperationException("Failed to deserialize YAML spec");

        var entityName = GetDictValue<string>(data, "entityName")
            ?? throw new InvalidOperationException("Missing entityName in spec");
        var moduleName = GetDictValue<string>(data, "moduleName")
            ?? throw new InvalidOperationException("Missing moduleName in spec");

        var hasSoftDelete = GetDictValue<bool?>(data, "hasSoftDelete") == true;
        var hasAuditFields = GetDictValue<bool?>(data, "hasAuditFields") != false;

        var schema = new EntitySchema
        {
            EntityName = entityName,
            ModuleName = moduleName,
            HasSoftDelete = hasSoftDelete,
            HasAuditFields = hasAuditFields,
            Description = GetDictValue<string>(data, "description"),
        };

        if (data.TryGetValue("fields", out var fieldsObj) && fieldsObj is List<object> fieldsList)
        {
            foreach (var fieldObj in fieldsList)
            {
                if (fieldObj is Dictionary<object, object> fieldDict)
                {
                    schema.Fields.Add(new EntityField
                    {
                        Name = GetDictValue<string>(fieldDict, "name") ?? "",
                        Type = GetDictValue<string>(fieldDict, "type") ?? "string",
                        IsRequired = GetDictValue<bool?>(fieldDict, "required") == true,
                        IsPrimaryKey = GetDictValue<bool?>(fieldDict, "isPrimaryKey") == true,
                        DefaultValue = GetDictValue<string>(fieldDict, "defaultValue"),
                        Validation = GetDictValue<string>(fieldDict, "validation"),
                    });
                }
            }
        }

        if (data.TryGetValue("tags", out var tagsObj) && tagsObj is List<object> tagsList)
        {
            schema.Tags = tagsList.OfType<string>().ToList();
        }

        return schema;
    }

    private T? GetDictValue<T>(Dictionary<object, object> dict, string key)
    {
        if (dict.TryGetValue(key, out var value))
        {
            return (T?)Convert.ChangeType(value, typeof(T));
        }
        return default;
    }
}

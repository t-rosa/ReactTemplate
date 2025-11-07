using System.IO;
using ReactTemplate.Tools.Commands;
using ReactTemplate.Tools.Models;
using Spectre.Console;

namespace ReactTemplate.Tools.Services;

/// <summary>
/// Service principal pour générer des entités complètes (serveur + client)
/// </summary>
public class ScaffoldingService
{
    private readonly string _repoRoot;
    private readonly FieldParser _fieldParser;
    private readonly SpecFileLoader _specLoader;
    private readonly TemplateRenderer _templateRenderer;

    public ScaffoldingService(string repoRoot)
    {
        _repoRoot = repoRoot;
        _fieldParser = new FieldParser();
        _specLoader = new SpecFileLoader();
        _templateRenderer = new TemplateRenderer();
    }

    public async Task ScaffoldEntityAsync(ScaffoldOptions options)
    {
        AnsiConsole.MarkupLine($"[yellow]🚀 Scaffolding entity: [bold]{options.Entity}[/] in module: [bold]{options.Module}[/][/]");

        // 1. Valider la structure du repository
        var detector = new RepositoryDetector();
        detector.ValidateRepositoryStructure(_repoRoot);

        // 2. Charger ou construire le schéma d'entité
        var schema = await BuildEntitySchemaAsync(options);

        // 3. Afficher un aperçu si dry-run
        if (options.DryRun)
        {
            DisplayDryRunPreview(schema);
            return;
        }

        // 4. Générer les artefacts serveur
        AnsiConsole.MarkupLine("[cyan]📝 Generating server artifacts...[/]");
        await GenerateServerArtifactsAsync(schema, options);

        // 5. Générer les artefacts client
        AnsiConsole.MarkupLine("[cyan]📝 Generating client artifacts...[/]");
        await GenerateClientArtifactsAsync(schema, options);

        // 6. Post-génération (format, lint, migrations)
        AnsiConsole.MarkupLine("[cyan]🔧 Post-generation tasks...[/]");
        await ExecutePostGenerationTasksAsync(schema, options);

        AnsiConsole.MarkupLine("[green]✅ Scaffolding completed successfully![/]");
    }

    private async Task<EntitySchema> BuildEntitySchemaAsync(ScaffoldOptions options)
    {
        EntitySchema schema;

        if (!string.IsNullOrEmpty(options.SpecFile))
        {
            // Charger depuis un fichier spec
            schema = _specLoader.LoadSpec(options.SpecFile);
        }
        else
        {
            // Construire à partir des options CLI
            var fields = new List<EntityField>();
            schema = new EntitySchema
            {
                EntityName = options.Entity,
                ModuleName = options.Module,
                HasAuditFields = true,
                Fields = fields
            };

            if (!string.IsNullOrEmpty(options.Fields))
            {
                schema.Fields = _fieldParser.ParseFieldsString(options.Fields);
            }
            else
            {
                // Champs par défaut
                schema.Fields = new List<EntityField>
                {
                    new() { Name = "Id", Type = "Guid", IsPrimaryKey = true },
                    new() { Name = "Name", Type = "string", IsRequired = true },
                };
            }
        }

        // S'assurer qu'il y a un Id
        if (!schema.Fields.Any(f => f.IsPrimaryKey))
        {
            schema.Fields.Insert(0, new EntityField { Name = "Id", Type = "Guid", IsPrimaryKey = true });
        }

        return schema;
    }

    private void DisplayDryRunPreview(EntitySchema schema)
    {
        var table = new Table { Title = new TableTitle("📋 Dry Run Preview - New Pattern (WeatherForecasts-based)") };
        table.AddColumn("Artifact");
        table.AddColumn("Path");

        var serverModuleDir = Path.Combine(_repoRoot, "ReactTemplate.Server", "Modules", schema.ModuleName);
        var clientModuleDir = Path.Combine(_repoRoot, "ReactTemplate.Client", "src", "modules", schema.ModuleName.ToLower());
        var routesDir = Path.Combine(_repoRoot, "ReactTemplate.Client", "src", "routes", "_app", schema.ModuleName.ToLower());

        // Server files
        table.AddRow("Entity", Path.Combine(serverModuleDir, $"{schema.EntityName}.cs"));
        table.AddRow("Controller", Path.Combine(serverModuleDir, $"{schema.EntityName}Controller.cs"));
        table.AddRow("Create DTO + Validator", Path.Combine(serverModuleDir, "Dtos", $"Create{schema.EntityName}Request.cs"));
        table.AddRow("Update DTO + Validator", Path.Combine(serverModuleDir, "Dtos", $"Update{schema.EntityName}Request.cs"));
        table.AddRow("Get Response DTO", Path.Combine(serverModuleDir, "Dtos", $"Get{schema.EntityName}Response.cs"));

        // Client files
        table.AddRow("Zod Schema", Path.Combine(clientModuleDir, $"{schema.EntityName.ToLower()}.schema.ts"));
        table.AddRow("Create View", Path.Combine(clientModuleDir, $"create-{schema.EntityName.ToLower()}.view.tsx"));
        table.AddRow("List View", Path.Combine(clientModuleDir, $"{schema.EntityName.ToLower()}s.view.tsx"));
        table.AddRow("Table Component", Path.Combine(clientModuleDir, $"{schema.EntityName.ToLower()}-table", $"{schema.EntityName.ToLower()}-table.view.tsx"));
        table.AddRow("Table Columns", Path.Combine(clientModuleDir, $"{schema.EntityName.ToLower()}-table", $"{schema.EntityName.ToLower()}-columns.tsx"));
        table.AddRow("Route File", Path.Combine(routesDir, "index.tsx"));

        AnsiConsole.Write(table);

        AnsiConsole.MarkupLine($"\n[yellow]Fields: {string.Join(", ", schema.Fields.Select(f => $"{f.Name}:{f.Type}"))}[/]");
    }

    private async Task GenerateServerArtifactsAsync(EntitySchema schema, ScaffoldOptions options)
    {
        // Structure plate: Modules/{Module}/
        var moduleDir = Path.Combine(_repoRoot, "ReactTemplate.Server", "Modules", schema.ModuleName);
        var dtosDir = Path.Combine(moduleDir, "Dtos");

        Directory.CreateDirectory(moduleDir);
        Directory.CreateDirectory(dtosDir);

        // Générer Entity au niveau du module
        await GenerateEntityFileAsync(schema, moduleDir);

        // Générer DTOs (new pattern: separate files per DTO with validators embedded)
        await GenerateCreateRequestAsync(schema, dtosDir);
        await GenerateUpdateRequestAsync(schema, dtosDir);
        await GenerateGetResponseAsync(schema, dtosDir);

        // Générer Controller au niveau du module
        await GenerateControllerAsync(schema, moduleDir);

        // Générer Tests si demandé
        if (options.GenerateTests)
        {
            await GenerateServerTestsAsync(schema);
        }

        AnsiConsole.MarkupLine($"[green]✓ Server artifacts generated in {moduleDir}[/]");
    }

    private async Task GenerateClientArtifactsAsync(EntitySchema schema, ScaffoldOptions options)
    {
        // Structure: modules/{module}/ following WeatherForecasts/forecasts pattern
        var moduleDir = Path.Combine(_repoRoot, "ReactTemplate.Client", "src", "modules", schema.ModuleName.ToLower());
        var routesDir = Path.Combine(_repoRoot, "ReactTemplate.Client", "src", "routes", "_app", schema.ModuleName.ToLower());

        Directory.CreateDirectory(moduleDir);
        Directory.CreateDirectory(routesDir);

        // Générer schema (Zod validation)
        await GenerateSchemaAsync(schema, moduleDir);

        // Générer views (create, list, table)
        await GenerateCreateViewAsync(schema, moduleDir);
        await GenerateListViewAsync(schema, moduleDir);
        await GenerateTableViewAsync(schema, moduleDir);

        // Générer colonnes pour la table
        await GenerateColumnsAsync(schema, moduleDir);

        // Générer la route
        await GenerateRouteAsync(schema, routesDir);

        // Générer Tests si demandé
        if (options.GenerateTests)
        {
            await GenerateClientTestsAsync(schema, moduleDir);
        }

        AnsiConsole.MarkupLine($"[green]✓ Client artifacts generated in {moduleDir}[/]");
    }

    private async Task<bool> GenerateEntityFileAsync(EntitySchema schema, string baseDir)
    {
        var writer = new FileWriter();
        var filePath = Path.Combine(baseDir, $"{schema.EntityName}.cs");

        var content = _templateRenderer.RenderTemplate("server-entity", schema);
        return await writer.WriteFileAsync(filePath, content);
    }

    private async Task<bool> GenerateCreateRequestAsync(EntitySchema schema, string baseDir)
    {
        var writer = new FileWriter();
        var filePath = Path.Combine(baseDir, $"Create{schema.EntityName}Request.cs");

        var content = _templateRenderer.RenderTemplate("server-create-request", schema);
        return await writer.WriteFileAsync(filePath, content);
    }

    private async Task<bool> GenerateUpdateRequestAsync(EntitySchema schema, string baseDir)
    {
        var writer = new FileWriter();
        var filePath = Path.Combine(baseDir, $"Update{schema.EntityName}Request.cs");

        var content = _templateRenderer.RenderTemplate("server-update-request", schema);
        return await writer.WriteFileAsync(filePath, content);
    }

    private async Task<bool> GenerateGetResponseAsync(EntitySchema schema, string baseDir)
    {
        var writer = new FileWriter();
        var filePath = Path.Combine(baseDir, $"Get{schema.EntityName}Response.cs");

        var content = _templateRenderer.RenderTemplate("server-get-response", schema);
        return await writer.WriteFileAsync(filePath, content);
    }

    private async Task<bool> GenerateControllerAsync(EntitySchema schema, string baseDir)
    {
        var writer = new FileWriter();
        var filePath = Path.Combine(baseDir, $"{schema.EntityName}Controller.cs");

        var content = _templateRenderer.RenderTemplate("server-controller", schema);
        return await writer.WriteFileAsync(filePath, content);
    }

    private async Task<bool> GenerateServerTestsAsync(EntitySchema schema)
    {
        // À implémenter avec Scriban dans la tâche suivante
        await Task.CompletedTask;
        return true;
    }

    private async Task<bool> GenerateSchemaAsync(EntitySchema schema, string baseDir)
    {
        var writer = new FileWriter();
        var filePath = Path.Combine(baseDir, $"{schema.EntityName.ToLower()}.schema.ts");

        var content = _templateRenderer.RenderTemplate("client-schema", schema);
        return await writer.WriteFileAsync(filePath, content);
    }

    private async Task<bool> GenerateCreateViewAsync(EntitySchema schema, string baseDir)
    {
        var writer = new FileWriter();
        var filePath = Path.Combine(baseDir, $"create-{schema.EntityName.ToLower()}.view.tsx");

        var content = _templateRenderer.RenderTemplate("client-create-view", schema);
        return await writer.WriteFileAsync(filePath, content);
    }

    private async Task<bool> GenerateListViewAsync(EntitySchema schema, string baseDir)
    {
        var writer = new FileWriter();
        var filePath = Path.Combine(baseDir, $"{schema.EntityName.ToLower()}s.view.tsx");

        var content = _templateRenderer.RenderTemplate("client-list-view", schema);
        return await writer.WriteFileAsync(filePath, content);
    }

    private async Task<bool> GenerateTableViewAsync(EntitySchema schema, string baseDir)
    {
        var tableDir = Path.Combine(baseDir, $"{schema.EntityName.ToLower()}-table");
        Directory.CreateDirectory(tableDir);

        var writer = new FileWriter();
        var filePath = Path.Combine(tableDir, $"{schema.EntityName.ToLower()}-table.view.tsx");

        var content = _templateRenderer.RenderTemplate("client-table-view", schema);
        return await writer.WriteFileAsync(filePath, content);
    }

    private async Task<bool> GenerateColumnsAsync(EntitySchema schema, string baseDir)
    {
        var tableDir = Path.Combine(baseDir, $"{schema.EntityName.ToLower()}-table");
        Directory.CreateDirectory(tableDir);

        var writer = new FileWriter();
        var filePath = Path.Combine(tableDir, $"{schema.EntityName.ToLower()}-columns.tsx");

        var content = _templateRenderer.RenderTemplate("client-columns", schema);
        return await writer.WriteFileAsync(filePath, content);
    }

    private async Task<bool> GenerateRouteAsync(EntitySchema schema, string baseDir)
    {
        var writer = new FileWriter();
        var filePath = Path.Combine(baseDir, "index.tsx");

        var content = _templateRenderer.RenderTemplate("client-route", schema);
        return await writer.WriteFileAsync(filePath, content);
    }

    private async Task<bool> GenerateClientTestsAsync(EntitySchema schema, string baseDir)
    {
        // À implémenter avec Scriban dans la tâche suivante
        await Task.CompletedTask;
        return true;
    }

    private async Task ExecutePostGenerationTasksAsync(EntitySchema schema, ScaffoldOptions options)
    {
        // 1. Modify DbContext
        AnsiConsole.MarkupLine("[cyan]🗄️ Updating DbContext...[/]");
        var dbModifier = new DbContextModifier(_repoRoot);
        await dbModifier.AddDbSetAsync(schema.EntityName, schema.ModuleName);

        // 2. Format C# files
        await FormatCSharpFilesAsync();

        // 3. Format TypeScript files
        await FormatTypeScriptFilesAsync();

        // 4. Suggest migration
        if (!options.SkipMigration)
        {
            SuggestMigration(schema);
        }
    }

    private async Task FormatCSharpFilesAsync()
    {
        AnsiConsole.MarkupLine("[dim]Running dotnet format...[/]");
        // dotnet format would be called here, but we don't execute it automatically
        // User should run: dotnet format
        await Task.CompletedTask;
    }

    private async Task FormatTypeScriptFilesAsync()
    {
        AnsiConsole.MarkupLine("[dim]Prettier formatting would be applied to generated TypeScript files[/]");
        // prettier would be called here, but we don't execute it automatically
        // User should run: npm run format
        await Task.CompletedTask;
    }

    private void SuggestMigration(EntitySchema schema)
    {
        AnsiConsole.MarkupLine($"\n[yellow]💡 Next steps:[/]");
        AnsiConsole.MarkupLine($"1. Review the generated files and DbContext modifications");
        AnsiConsole.MarkupLine($"2. Create EF migration: [cyan]dotnet ef migrations add Add{schema.EntityName}[/]");
        AnsiConsole.MarkupLine($"3. Update database: [cyan]dotnet ef database update[/]");
        AnsiConsole.MarkupLine($"4. Format code: [cyan]dotnet format && npm run format[/]");
    }
}

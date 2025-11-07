using ReactTemplate.Tools.Commands;
using ReactTemplate.Tools.Services;
using Spectre.Console;

namespace ReactTemplate.Tools;

class Program
{
    static async Task<int> Main(string[] args)
    {
        try
        {
            if (args.Length == 0 || args[0] == "help" || args[0] == "--help" || args[0] == "-h")
            {
                ShowHelp();
                return 0;
            }

            if (args[0] == "scaffold")
            {
                return await HandleScaffold(args.Skip(1).ToArray());
            }

            AnsiConsole.MarkupLine("[red]Unknown command[/]");
            ShowHelp();
            return 1;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ Error: {ex.Message}[/]");
            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                Console.WriteLine(ex.StackTrace);
            }
            return 1;
        }
    }

    static async Task<int> HandleScaffold(string[] args)
    {
        var options = ParseScaffoldArgs(args);

        if (options == null)
        {
            ShowScaffoldHelp();
            return 1;
        }

        var repoDetector = new RepositoryDetector();
        var repoRoot = repoDetector.DetectRepositoryRoot();

        if (repoRoot == null)
        {
            AnsiConsole.MarkupLine("[red]❌ Could not find ReactTemplate.slnx in current directory or parents.[/]");
            return 1;
        }

        var scaffoldService = new ScaffoldingService(repoRoot);
        await scaffoldService.ScaffoldEntityAsync(options);
        return 0;
    }

    static ScaffoldOptions? ParseScaffoldArgs(string[] args)
    {
        var options = new ScaffoldOptions
        {
            Module = "",
            Entity = "",
            GenerateTests = true,
            SkipMigration = false,
            DryRun = false
        };

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--module" or "-m":
                    if (i + 1 < args.Length)
                        options.Module = args[++i];
                    break;
                case "--entity" or "-e":
                    if (i + 1 < args.Length)
                        options.Entity = args[++i];
                    break;
                case "--fields" or "-f":
                    if (i + 1 < args.Length)
                        options.Fields = args[++i];
                    break;
                case "--spec" or "-s":
                    if (i + 1 < args.Length)
                        options.SpecFile = args[++i];
                    break;
                case "--skip-tests":
                    options.GenerateTests = false;
                    break;
                case "--with-tests":
                    options.GenerateTests = true;
                    break;
                case "--skip-migration":
                    options.SkipMigration = true;
                    break;
                case "--dry-run":
                    options.DryRun = true;
                    break;
                case "--help" or "-h":
                    ShowScaffoldHelp();
                    return null;
            }
        }

        if (string.IsNullOrEmpty(options.Module) || string.IsNullOrEmpty(options.Entity))
        {
            AnsiConsole.MarkupLine("[red]Error: --module and --entity are required[/]");
            return null;
        }

        return options;
    }

    static void ShowHelp()
    {
        Console.WriteLine(@"
╔════════════════════════════════════════════════════════════════════════════╗
║            ReactTemplate Scaffolding CLI - Generate CRUD Entities           ║
╚════════════════════════════════════════════════════════════════════════════╝

USAGE:
  react-template-scaffold <command> [args]

COMMANDS:
  scaffold              Generate a new entity with server and client code
  help                  Show this help message

EXAMPLES:
  react-template-scaffold scaffold --module Products --entity Product
  react-template-scaffold scaffold --module Products --entity Product \\
    --fields ""name:string required; price:decimal; stock:int""
  react-template-scaffold scaffold --spec ./specs/product.json --dry-run

Run 'react-template-scaffold scaffold --help' for more details
");
    }

    static void ShowScaffoldHelp()
    {
        Console.WriteLine(@"
╔════════════════════════════════════════════════════════════════════════════╗
║                        Scaffold Command - Entity Generator                  ║
╚════════════════════════════════════════════════════════════════════════════╝

USAGE:
  react-template-scaffold scaffold --module <MODULE> --entity <ENTITY> [options]

REQUIRED OPTIONS:
  -m, --module <MODULE>        Module name (e.g., Products, WeatherForecasts)
  -e, --entity <ENTITY>        Entity name (e.g., Product, WeatherForecast)

OPTIONAL OPTIONS:
  -f, --fields <FIELDS>        Field definitions
                               (e.g., 'name:string required; active:bool')
  -s, --spec <FILE>            Path to JSON/YAML spec file
  --with-tests                 Generate test files (default: true)
  --skip-tests                 Skip test file generation
  --skip-migration             Skip migration suggestion
  --dry-run                    Preview without creating files
  -h, --help                   Show this help

SUPPORTED FIELD TYPES:
  string, int, decimal, bool, datetime, guid, long, double, float

EXAMPLES:
  # Simple inline fields
  react-template-scaffold scaffold -m Products -e Product \\
    -f ""name:string required; price:decimal; stock:int default=0""

  # From spec file
  react-template-scaffold scaffold -m Products -e Product \\
    --spec ./specs/product.json

  # Preview mode
  react-template-scaffold scaffold -m Products -e Product --dry-run

  # Without tests
  react-template-scaffold scaffold -m Products -e Product --skip-tests

For more info: https://github.com/t-rosa/ReactTemplate
");
    }
}

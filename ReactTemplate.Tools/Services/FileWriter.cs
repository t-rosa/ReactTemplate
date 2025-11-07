using Spectre.Console;

namespace ReactTemplate.Tools.Services;

/// <summary>
/// Service for safely writing generated files with validation and conflict detection
/// </summary>
public class FileWriter
{
    private readonly List<string> _generatedFiles = new();
    private readonly bool _overwrite;

    public FileWriter(bool overwrite = false)
    {
        _overwrite = overwrite;
    }

    /// <summary>
    /// Get list of all files written during this session
    /// </summary>
    public IReadOnlyList<string> GeneratedFiles => _generatedFiles.AsReadOnly();

    /// <summary>
    /// Write content to a file with safety checks
    /// </summary>
    public async Task<bool> WriteFileAsync(string filePath, string content, bool createDirectories = true)
    {
        try
        {
            var directory = Path.GetDirectoryName(filePath);
            if (string.IsNullOrEmpty(directory))
            {
                AnsiConsole.MarkupLine($"[red]✗ Invalid file path: {filePath}[/]");
                return false;
            }

            // Create directories if needed
            if (createDirectories && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                AnsiConsole.MarkupLine($"[dim]  Created directory: {directory}[/]");
            }

            // Check for existing file
            if (File.Exists(filePath) && !_overwrite)
            {
                AnsiConsole.MarkupLine($"[yellow]⚠ File already exists (skipped): {Path.GetFileName(filePath)}[/]");
                return false;
            }

            // Write file
            await File.WriteAllTextAsync(filePath, content);
            _generatedFiles.Add(filePath);
            AnsiConsole.MarkupLine($"[green]✓ {Path.GetFileName(filePath)}[/]");

            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]✗ Error writing file {Path.GetFileName(filePath)}: {ex.Message}[/]");
            return false;
        }
    }

    /// <summary>
    /// Create a directory if it doesn't exist
    /// </summary>
    public bool CreateDirectory(string directoryPath)
    {
        try
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                AnsiConsole.MarkupLine($"[dim]  📁 {directoryPath}[/]");
                return true;
            }
            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]✗ Error creating directory: {ex.Message}[/]");
            return false;
        }
    }

    /// <summary>
    /// Check if a file exists
    /// </summary>
    public bool FileExists(string filePath) => File.Exists(filePath);

    /// <summary>
    /// Get file size
    /// </summary>
    public long GetFileSize(string filePath)
    {
        return FileExists(filePath) ? new FileInfo(filePath).Length : 0;
    }

    /// <summary>
    /// Delete a file (use with caution)
    /// </summary>
    public bool DeleteFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]✗ Error deleting file: {ex.Message}[/]");
            return false;
        }
    }

    /// <summary>
    /// Display summary of generated files
    /// </summary>
    public void DisplaySummary()
    {
        if (!_generatedFiles.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No files generated[/]");
            return;
        }

        AnsiConsole.MarkupLine($"\n[bold green]📦 Summary: {_generatedFiles.Count} file(s) generated[/]");

        var table = new Table { Title = new TableTitle("Generated Files") };
        table.AddColumn("File");
        table.AddColumn("Size");

        foreach (var file in _generatedFiles)
        {
            var size = GetFileSize(file);
            var sizeStr = size switch
            {
                < 1024 => $"{size} B",
                < 1024 * 1024 => $"{size / 1024} KB",
                _ => $"{size / (1024 * 1024)} MB"
            };

            table.AddRow(
                Path.GetFileName(file),
                sizeStr
            );
        }

        AnsiConsole.Write(table);
    }
}

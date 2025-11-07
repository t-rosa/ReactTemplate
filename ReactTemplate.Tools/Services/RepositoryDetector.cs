using System.IO;

namespace ReactTemplate.Tools.Services;

/// <summary>
/// Service pour détecter la racine du repository ReactTemplate
/// </summary>
public class RepositoryDetector
{
    private const string SlnFile = "ReactTemplate.slnx";

    /// <summary>
    /// Détecte la racine du repository en cherchant ReactTemplate.slnx
    /// </summary>
    /// <returns>Chemin vers la racine ou null si non trouvé</returns>
    public string? DetectRepositoryRoot()
    {
        var currentPath = Directory.GetCurrentDirectory();

        while (currentPath != Path.GetPathRoot(currentPath))
        {
            var slnPath = Path.Combine(currentPath, SlnFile);
            if (File.Exists(slnPath))
            {
                return currentPath;
            }

            currentPath = Directory.GetParent(currentPath)?.FullName ?? string.Empty;
            if (string.IsNullOrEmpty(currentPath))
            {
                break;
            }
        }

        return null;
    }

    /// <summary>
    /// Valide que la structure de répertoires existe
    /// </summary>
    public void ValidateRepositoryStructure(string repoRoot)
    {
        var requiredDirs = new[]
        {
            Path.Combine(repoRoot, "ReactTemplate.Server", "Modules"),
            Path.Combine(repoRoot, "ReactTemplate.Client", "src", "modules"),
        };

        foreach (var dir in requiredDirs)
        {
            if (!Directory.Exists(dir))
            {
                throw new DirectoryNotFoundException($"Required directory not found: {dir}");
            }
        }
    }
}

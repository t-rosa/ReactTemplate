namespace ReactTemplate.Server.Modules.Common;

/// <summary>
/// Interface pour les entités auditables.
/// Les entités implémentant cette interface auront leurs métadonnées de création/modification
/// automatiquement remplies lors de SaveChanges.
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// Date et heure de création de l'entité (UTC).
    /// </summary>
    DateTime CreatedAt { get; set; }

    /// <summary>
    /// Identifiant (Guid) de l'utilisateur ayant créé l'entité.
    /// </summary>
    Guid? CreatedBy { get; set; }

    /// <summary>
    /// Date et heure de la dernière modification de l'entité (UTC).
    /// </summary>
    DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Identifiant (Guid) de l'utilisateur ayant effectué la dernière modification.
    /// </summary>
    Guid? UpdatedBy { get; set; }

    /// <summary>
    /// Indique si l'entité est marquée comme supprimée (soft delete).
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Date et heure de la suppression logique (UTC).
    /// </summary>
    DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Identifiant (Guid) de l'utilisateur ayant effectué la suppression logique.
    /// </summary>
    Guid? DeletedBy { get; set; }
}

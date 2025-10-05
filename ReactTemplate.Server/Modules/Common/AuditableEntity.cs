namespace ReactTemplate.Server.Modules.Common;

/// <summary>
/// Classe de base abstraite pour les entités auditables.
/// Fournit les propriétés standard pour le tracking des créations et modifications.
/// </summary>
public abstract class AuditableEntity : IAuditableEntity
{
    /// <summary>
    /// Date et heure de création de l'entité (UTC).
    /// Automatiquement remplie lors de l'ajout de l'entité.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Identifiant (Guid) de l'utilisateur ayant créé l'entité.
    /// Automatiquement remplie lors de l'ajout de l'entité.
    /// </summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// Date et heure de la dernière modification de l'entité (UTC).
    /// Automatiquement mise à jour lors de la modification de l'entité.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Identifiant (Guid) de l'utilisateur ayant effectué la dernière modification.
    /// Automatiquement mise à jour lors de la modification de l'entité.
    /// </summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>
    /// Indique si l'entité est marquée comme supprimée (soft delete).
    /// Par défaut : false (non supprimée).
    /// Automatiquement mise à true lors de la suppression de l'entité.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Date et heure de la suppression logique (UTC).
    /// Automatiquement remplie lors de la suppression de l'entité.
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Identifiant (Guid) de l'utilisateur ayant effectué la suppression logique.
    /// Automatiquement remplie lors de la suppression de l'entité.
    /// </summary>
    public Guid? DeletedBy { get; set; }
}

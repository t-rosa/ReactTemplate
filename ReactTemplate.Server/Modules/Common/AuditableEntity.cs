namespace ReactTemplate.Server.Modules.Common;

/// <summary>
/// Abstract base class for auditable entities.
/// Provides standard properties for tracking creations and modifications.
/// </summary>
public abstract class AuditableEntity : IAuditableEntity
{
    /// <summary>
    /// Entity creation date and time (UTC).
    /// Automatically filled when adding the entity.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// User identifier (Guid) who created the entity.
    /// Automatically filled when adding the entity.
    /// </summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// Entity last modification date and time (UTC).
    /// Automatically updated when modifying the entity.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// User identifier (Guid) who made the last modification.
    /// Automatically updated when modifying the entity.
    /// </summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>
    /// Indicates if the entity is marked as deleted (soft delete).
    /// Default: false (not deleted).
    /// Automatically set to true when deleting the entity.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Logical deletion date and time (UTC).
    /// Automatically filled when deleting the entity.
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// User identifier (Guid) who performed the logical deletion.
    /// Automatically filled when deleting the entity.
    /// </summary>
    public Guid? DeletedBy { get; set; }
}

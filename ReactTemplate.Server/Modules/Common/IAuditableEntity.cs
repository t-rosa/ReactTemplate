namespace ReactTemplate.Server.Modules.Common;

/// <summary>
/// Interface for auditable entities.
/// Entities implementing this interface will have their creation/modification metadata
/// automatically filled in during SaveChanges.
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// Entity creation date and time (UTC).
    /// </summary>
    DateTime CreatedAt { get; set; }

    /// <summary>
    /// User identifier (Guid) who created the entity.
    /// </summary>
    Guid? CreatedBy { get; set; }

    /// <summary>
    /// Entity last modification date and time (UTC).
    /// </summary>
    DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// User identifier (Guid) who made the last modification.
    /// </summary>
    Guid? UpdatedBy { get; set; }

    /// <summary>
    /// Indicates whether the entity is marked as deleted (soft delete).
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Logical deletion date and time (UTC).
    /// </summary>
    DateTime? DeletedAt { get; set; }

    /// <summary>
    /// User identifier (Guid) who performed the logical deletion.
    /// </summary>
    Guid? DeletedBy { get; set; }
}

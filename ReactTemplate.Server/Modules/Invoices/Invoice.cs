using ReactTemplate.Server.Modules.Common;
using ReactTemplate.Server.Modules.Users;

namespace Invoices;

/// <summary>
/// Invoice entity
/// </summary>
public class Invoice : AuditableEntity
{
    public Guid Id { get; set; }

    /// <summary>Foreign key to User</summary>
    public Guid UserId { get; set; }

    /// <summary>Navigation property to User</summary>
    public User User { get; set; } = default!;
}

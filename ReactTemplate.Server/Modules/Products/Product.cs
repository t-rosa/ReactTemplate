using ReactTemplate.Server.Modules.Common;
using ReactTemplate.Server.Modules.Users;

namespace ReactTemplate.Server.Modules.Products;

/// <summary>
/// Product entity
/// </summary>
public class Product : AuditableEntity
{
    public Guid Id { get; set; }

    /// <summary>Foreign key to User</summary>
    public Guid UserId { get; set; }

    /// <summary>Navigation property to User</summary>
    public User User { get; set; } = default!;
}

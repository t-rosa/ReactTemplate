using Microsoft.AspNetCore.Identity;

namespace ReactTemplate.Server.Modules.Users;

public sealed class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

using Microsoft.AspNetCore.Identity;

namespace ReactTemplate.Authentication;

public class User : IdentityUser
{
    public string? FirstName { get; private set; }

    public string? LastName { get; private set; }
}

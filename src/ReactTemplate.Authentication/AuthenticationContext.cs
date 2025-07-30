using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ReactTemplate.Authentication;

public class AuthenticationContext(DbContextOptions<AuthenticationContext> options) : IdentityDbContext<User>(options)
{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("identity");

        base.OnModelCreating(modelBuilder);
    }
}

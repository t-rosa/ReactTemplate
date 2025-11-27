using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReactTemplate.Server.Modules.Auth.DTOs;

namespace ReactTemplate.Server.Modules.Users;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController(UserManager<User> userManager) : ControllerBase
{
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUser()
    {
        User? user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var response = new UserResponse
        {
            Id = await userManager.GetUserIdAsync(user) ?? throw new NotSupportedException("Users must have an Id."),
            Email = await userManager.GetEmailAsync(user) ?? throw new NotSupportedException("Users must have an email."),
            Roles = await userManager.GetRolesAsync(user) ?? throw new NotSupportedException("Users must have a role."),
            IsEmailConfirmed = await userManager.IsEmailConfirmedAsync(user)
        };

        return Ok(response);
    }
}

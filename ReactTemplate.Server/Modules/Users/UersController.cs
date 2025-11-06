using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReactTemplate.Server.Modules.Auth.Dtos;
using ReactTemplate.Server.Modules.Users;

namespace ReactTemplate.Server.Modules.Auth;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public UsersController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("me")]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var response = new GetUserResponse
        {
            Id = await _userManager.GetUserIdAsync(user) ?? throw new NotSupportedException("Users must have an Id."),
            Email = await _userManager.GetEmailAsync(user) ?? throw new NotSupportedException("Users must have an email."),
            Roles = await _userManager.GetRolesAsync(user) ?? throw new NotSupportedException("Users must have a role."),
            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user)
        };

        return Ok(response);
    }
}

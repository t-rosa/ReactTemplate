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

        var response = new GetUserResponse(await _userManager.GetUserIdAsync(user) ?? throw new NotSupportedException("Users must have an Id."), await _userManager.GetEmailAsync(user) ?? throw new NotSupportedException("Users must have an email."), await _userManager.GetRolesAsync(user) ?? throw new NotSupportedException("Users must have a role."), await _userManager.IsEmailConfirmedAsync(user));

        return Ok(response);
    }
}

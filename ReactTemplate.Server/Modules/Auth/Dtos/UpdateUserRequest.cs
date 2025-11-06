namespace ReactTemplate.Server.Modules.Auth.Dtos;

public class UpdateUserRequest
{
    public string NewEmail { get; set; } = string.Empty;

    public string NewPassword { get; set; } = string.Empty;

    public string OldPassword { get; set; } = string.Empty;
}

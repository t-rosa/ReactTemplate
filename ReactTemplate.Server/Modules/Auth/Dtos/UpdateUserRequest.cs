namespace ReactTemplate.Server.Modules.Auth.DTOs;

public sealed record UpdateUserRequest
{
    public required string NewEmail { get; init; }
    public required string NewPassword { get; init; }
    public required string OldPassword { get; init; }
}

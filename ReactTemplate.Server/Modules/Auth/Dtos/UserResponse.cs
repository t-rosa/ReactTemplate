namespace ReactTemplate.Server.Modules.Auth.DTOs;

public sealed record UserResponse
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required IList<string> Roles { get; init; }
    public required bool IsEmailConfirmed { get; init; }
}

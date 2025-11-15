namespace ReactTemplate.Server.Modules.Auth.Dtos;

public sealed record GetUserResponse
{
    public string Id { get; init; }
    public string Email { get; init; }
    public IList<string> Roles { get; init; }
    public bool IsEmailConfirmed { get; init; }
}

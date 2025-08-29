namespace ReactTemplate.Server.Modules.Auth.Dtos;

public class GetUserResponse
{
    public required string Id { get; set; }

    public required string Email { get; set; }

    public required IList<string> Roles { get; set; }

    public required bool IsEmailConfirmed { get; set; }
}

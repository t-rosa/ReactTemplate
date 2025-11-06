namespace ReactTemplate.Server.Modules.Auth.Dtos;

public class GetUserResponse
{
    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public IList<string> Roles { get; set; } = new List<string>();

    public bool IsEmailConfirmed { get; set; }
}

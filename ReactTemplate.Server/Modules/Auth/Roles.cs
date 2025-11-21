namespace ReactTemplate.Server.Modules.Auth;

public static class Roles
{
    public const string Admin = nameof(Admin);
    public const string Member = nameof(Member);

    public static string[] All { get; set; } = [Admin, Member];
}

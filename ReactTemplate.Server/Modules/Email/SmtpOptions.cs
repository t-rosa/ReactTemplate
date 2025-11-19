namespace ReactTemplate.Server.Modules.Email;

public sealed record SmtpOptions
{
    public required string Server { get; init; }
    public required int Port { get; init; }
    public required bool EnableSSL { get; init; }
}

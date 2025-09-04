namespace JobScheduling.Persistence.Application.Email;

internal sealed class SmtpSettings
{
    public required string Host { get; init; }
    public required int Port { get; init; }
    public bool EnableSsl { get; init; }
    public string? Username { get; init; }
    public string? Password { get; init; }
}
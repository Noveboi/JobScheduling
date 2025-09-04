using Serilog;
using ILogger = Serilog.ILogger;

namespace JobScheduling.RestApi.Services;

/// <summary>
/// A mock email service, just outputs a log message after waiting for 1 second.
/// </summary>
internal sealed class EmailService : IEmailService
{
    private readonly ILogger _log = Log.ForContext<EmailService>();
    
    public async Task SendEmailAsync(string message)
    {
        await Task.Delay(1000);
        _log.Information("Sent email: {email}", message);
    }
}
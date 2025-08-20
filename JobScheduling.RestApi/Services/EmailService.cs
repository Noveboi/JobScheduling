using Serilog;
using ILogger = Serilog.ILogger;

namespace JobScheduling.RestApi.Services;

internal sealed class EmailService : IEmailService
{
    private readonly ILogger _log = Log.ForContext<EmailService>();
    
    public async Task SendEmailAsync(string message)
    {
        await Task.Delay(1000);
        _log.Information("Sent email: {email}", message);
    }
}
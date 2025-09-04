using JobScheduling.Persistence.Domain;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace JobScheduling.Persistence.Application.Email;

internal sealed class EmailService(IOptions<SmtpSettings> settings, ILogger<EmailService> log)
{
    private readonly SmtpSettings _settings = settings.Value;
    
    public async Task SendAsync(User to, string subject, string body, CancellationToken ct)
    {
        log.LogInformation("Sending email to {userName}", to.Name);
        
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Job Scheduling!", "noreply@novesoftware.net"));
        message.To.Add(new MailboxAddress(name: to.Name, address: to.EmailAddress));

        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Plain)
        {
            Text = body
        };

        using var client = new MailKit.Net.Smtp.SmtpClient();
        await client.ConnectAsync(
            host: _settings.Host,
            port: _settings.Port,
            useSsl: _settings.EnableSsl,
            cancellationToken: ct);

        if (!string.IsNullOrWhiteSpace(_settings.Username))
        {
            await client.AuthenticateAsync(
                userName: _settings.Username,
                password: _settings.Password,
                cancellationToken: ct);
        }

        await client.SendAsync(message, ct);
        await client.DisconnectAsync(quit: true, ct);
    }
}
using JobScheduling.Persistence.Domain;

namespace JobScheduling.Persistence.Application.Email;

internal interface IEmailService
{
    Task SendAsync(User to, string subject, string body, CancellationToken ct);
}
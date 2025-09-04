namespace JobScheduling.RestApi.Services;

public interface IEmailService
{
    Task SendEmailAsync(string message);
}
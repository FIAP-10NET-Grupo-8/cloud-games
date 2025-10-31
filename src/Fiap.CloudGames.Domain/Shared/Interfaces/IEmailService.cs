namespace Fiap.CloudGames.Domain.Shared.Interfaces;

public interface IEmailService
{
	Task SendEmailAsync(string email, string subject, string htmlMessage);
}

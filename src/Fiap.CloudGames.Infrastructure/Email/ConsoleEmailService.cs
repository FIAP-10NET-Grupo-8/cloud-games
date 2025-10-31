using Fiap.CloudGames.Domain.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace Fiap.CloudGames.Infrastructure.Email;

public class ConsoleEmailService(ILogger<ConsoleEmailService> logger) : IEmailService
{
	private readonly ILogger<ConsoleEmailService> logger = logger;

	public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        logger.LogInformation("=== Sending Email ===\nTo: {email}\nSubject: {subject}\nMessage:\n{htmlMessage}\n=====================", email, subject, htmlMessage);
		return Task.CompletedTask;
    }
}

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace Fiap.CloudGames.Infrastructure.Email;

public class ConsoleEmailSender(ILogger<ConsoleEmailSender> logger) : IEmailSender
{
	private readonly ILogger<ConsoleEmailSender> logger = logger;

	public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        logger.LogInformation("=== Sending Email ===\nTo: {email}\nSubject: {subject}\nMessage:\n{htmlMessage}\n=====================", email, subject, htmlMessage);
		return Task.CompletedTask;
    }
}

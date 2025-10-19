using Microsoft.AspNetCore.Identity.UI.Services;

namespace Fiap.CloudGames.Infrastructure.Email;

public class ConsoleEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        Console.WriteLine("=== Sending Email ===");
        Console.WriteLine($"To: {email}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine("Message:");
        Console.WriteLine(htmlMessage);
        Console.WriteLine("=====================");
        return Task.CompletedTask;
    }
}

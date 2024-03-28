using CleanArchitecture.Application.Abstractions.Email;

namespace CleanArchitecture.Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    public Task SendEmailAsync(Domain.Users.Email recipient, string subject, string message)
    {
        // Implementación de envío de correo electrónico
        return Task.CompletedTask;
    }
}
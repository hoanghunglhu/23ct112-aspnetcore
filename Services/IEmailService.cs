using System;

namespace LearnApiNetCore.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlBody, List<string>? attachmentPaths = null);
        Task SendEmailAsync(List<string> toEmails, string subject, string htmlBody, List<string>? attachmentPaths = null);
    }
}
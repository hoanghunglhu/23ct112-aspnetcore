using System.Threading.Tasks;

namespace LearnApiNetCore.Services
{
    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(EmailMessage mail)
        {
            // TODO: Implement actual email sending logic here
            return Task.CompletedTask;
        }
    }
}
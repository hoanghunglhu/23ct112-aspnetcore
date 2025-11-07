using System.Threading.Tasks;

namespace LearnApiNetCore.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage mail);
    }
}
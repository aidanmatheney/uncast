namespace Uncast.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
    }
}
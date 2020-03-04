namespace Uncast.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using Uncast.Utils;

    public sealed class ToLogEmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public ToLogEmailSender(ILogger<ToLogEmailSender> logger)
        {
            ThrowIf.Null(logger, nameof(logger));

            _logger = logger;
        }

        public Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        {
            _logger.LogWarning($"Sending email to {{to}}:{Environment.NewLine}{{subject}}:{Environment.NewLine}{{body}}", to, subject, body);
            return Task.CompletedTask;
        }
    }
}
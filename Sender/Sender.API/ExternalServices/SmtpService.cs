using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Extensions;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Sender.API.Sections;

namespace Sender.API.ExternalServices
{
    public sealed class SmtpService : ISmtpService
    {
        private readonly SmtpClient _smtpClient;
        private readonly ILogger<SmtpService> _logger;
        private readonly IOptions<SmtpSection> _smtpOptions;

        public SmtpService(SmtpClient smtpClient, ILogger<SmtpService> logger, IOptions<SmtpSection> smtpOptions)
        {
            _smtpClient = smtpClient;
            _logger = logger;
            _smtpOptions = smtpOptions;
        }

        public async Task SendMailAsync(IEnumerable<string> receivers, string subject, string body)
        {
            receivers = receivers.Where(r => !string.IsNullOrWhiteSpace(r)).ToArray();

            var message = new MimeMessage(
                from: new [] { MailboxAddress.Parse(_smtpOptions.Value.From) },
                to: receivers.Select(MailboxAddress.Parse),
                subject: subject,
                body: BuildBody(body));

            if (!_smtpClient.IsConnected)
            {
                await _smtpClient.ConnectAsync(_smtpOptions.Value.Host, _smtpOptions.Value.Port, false);
            }

            if (!_smtpClient.IsAuthenticated)
            {
                await _smtpClient.AuthenticateAsync(string.Empty, string.Empty);
            }

            _logger.LogInformation("Start sending email to {To}", receivers.JoinString());

            await _smtpClient.SendAsync(message);

            _logger.LogInformation("End sending email to {To}", receivers.JoinString());
        }

        private static MimeEntity BuildBody(string body)
        {
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            return bodyBuilder.ToMessageBody();
        }
    }


}

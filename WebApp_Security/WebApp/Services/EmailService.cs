using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using WebApp.Settings;

namespace WebApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<SMTPSettings> _smtpOptions;

        public EmailService(IOptions<SMTPSettings> smtpOptions)
        {
            _smtpOptions = smtpOptions;
        }

        public async Task SendAsync(string from, string to, string subject, string body)
        {
            var message = new MailMessage(from, to, subject, body);

            using (var emailClient = new SmtpClient(_smtpOptions.Value.host, _smtpOptions.Value.port))
            {
                emailClient.Credentials = new NetworkCredential(_smtpOptions.Value.username, _smtpOptions.Value.password);

                await emailClient.SendMailAsync(message);

            }
        }
    }
}

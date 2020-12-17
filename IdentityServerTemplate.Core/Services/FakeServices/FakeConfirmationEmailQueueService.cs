using IdentityServerTemplate.Core.DTOs;
using IdentityServerTemplate.Shared.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServerTemplate.Core.Services
{
    public class FakeConfirmationEmailQueueService : IStorageQueue<ConfirmationEmailDTO>
    {
        private readonly IConfiguration _configuration;

        public FakeConfirmationEmailQueueService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Enqueue(ConfirmationEmailDTO obj, CancellationToken ct)
        {
            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress("admin@esfer.com")
                };

                mail.To.Add(new MailAddress(obj.Email));

                mail.Subject = $"Welcome to Esfer {obj.UserName} Email Confirmation";
                mail.Body = $"Please confirm your account by <a href='{obj.ConfirmationLink}'>clicking here</a>.";
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using SmtpClient smtp = new SmtpClient(
                    _configuration.GetValue<string>("SMTPSettings:Domain"),
                    _configuration.GetValue<int>("SMTPSettings:Port"))
                {
                    EnableSsl = false
                };
                await smtp.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw ex; //TODO: retry service
            }
        }
    }
}

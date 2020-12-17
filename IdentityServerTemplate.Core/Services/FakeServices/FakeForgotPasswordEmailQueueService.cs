using IdentityServerTemplate.Core.DTOs;
using IdentityServerTemplate.Shared.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServerTemplate.Core.Services.FakeServices
{
    public class FakeForgotPasswordEmailQueueService : IStorageQueue<ForgotPasswordEmailDTO>
    {
        private readonly IConfiguration _configuration;

        public FakeForgotPasswordEmailQueueService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Enqueue(ForgotPasswordEmailDTO obj, CancellationToken ct)
        {
            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress("admin@esfer.com")
                };

                mail.To.Add(new MailAddress(obj.Email));

                mail.Subject = $"Password Reset Request";
                mail.Body = $"Please follow this link to reste your password: <a href='{obj.ConfirmationLink}'>clicking here</a>.";
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

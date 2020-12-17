using IdentityServerTemplate.Core.DTOs;
using IdentityServerTemplate.Core.Entities;
using IdentityServerTemplate.Core.Enums;
using IdentityServerTemplate.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServerTemplate.Core.Events.Handlers
{
    public class SendForgotPasswordEmailEventHandler : INotificationHandler<SendForgotPasswordEmailEvent>
    {
        private readonly UserManager<Account> _accountManager;
        private readonly IStorageQueue<ForgotPasswordEmailDTO> _queueService;
        private readonly IConfiguration _configuration;

        public SendForgotPasswordEmailEventHandler(UserManager<Account> accountManager, IStorageQueue<ForgotPasswordEmailDTO> queueService, IConfiguration configuration)
        {
            _accountManager = accountManager;
            _queueService = queueService;
            _configuration = configuration;
        }

        public async Task Handle(SendForgotPasswordEmailEvent notification, CancellationToken cancellationToken)
        {
            var confirmationToken = await _accountManager.GenerateEmailConfirmationTokenAsync(notification.Account);

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));

            var confirmationLink = $"{_configuration.GetSection("BaseAPIUrl").Value}v1/user/reset-password?token={encodedToken}";

            await _queueService.Enqueue(new ForgotPasswordEmailDTO
            {
                EmailTemplate = EmailTemplates.ForgotPasswordEmail,
                ConfirmationLink = HtmlEncoder.Default.Encode(confirmationLink),
                Email = notification.Account.Email,
                UserName = notification.Account.UserName
            }, cancellationToken);
        }
    }
}

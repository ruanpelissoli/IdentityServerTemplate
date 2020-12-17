using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using IdentityServerTemplate.Core.DTOs;
using IdentityServerTemplate.Core.Entities;
using IdentityServerTemplate.Core.Enums;
using IdentityServerTemplate.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace IdentityServerTemplate.Core.Events.Handlers
{
    public class SendConfirmationEmailEventHandler : INotificationHandler<SendConfirmationEmailEvent>
    {
        private readonly UserManager<Account> _accountManager;
        private readonly IStorageQueue<ConfirmationEmailDTO> _queueService;
        private readonly IConfiguration _configuration;

        public SendConfirmationEmailEventHandler(UserManager<Account> accountManager, IStorageQueue<ConfirmationEmailDTO> queueService, IConfiguration configuration)
        {
            _accountManager = accountManager;
            _queueService = queueService;
            _configuration = configuration;
        }

        public async Task Handle(SendConfirmationEmailEvent notification, CancellationToken cancellationToken)
        {
            var confirmationToken = await _accountManager.GenerateEmailConfirmationTokenAsync(notification.Account);

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));

            var confirmationLink = $"{_configuration.GetSection("BaseAPIUrl").Value}v1/user/email-confirmation?token={encodedToken}&id={notification.Account.Id}";

            await _queueService.Enqueue(new ConfirmationEmailDTO
            {
                EmailTemplate = EmailTemplates.ConfirmationEmail,
                ConfirmationLink = HtmlEncoder.Default.Encode(confirmationLink),
                Email = notification.Account.Email,
                UserName = notification.Account.UserName
            }, cancellationToken);            
        }
    }
}
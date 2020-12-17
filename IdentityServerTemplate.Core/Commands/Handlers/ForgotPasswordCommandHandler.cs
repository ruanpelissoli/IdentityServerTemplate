using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using IdentityServerTemplate.Core.DTOs;
using IdentityServerTemplate.Core.Events;
using IdentityServerTemplate.Core.Repositories;
using IdentityServerTemplate.Shared.Infrastructure;
using IdentityServerTemplate.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace IdentityServerTemplate.Core.Commands.Handlers
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ApiResponse>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IStorageQueue<ConfirmationEmailDTO> _emailQueueService;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public ForgotPasswordCommandHandler(
            IAccountRepository accountRepository, 
            IStorageQueue<ConfirmationEmailDTO> emailQueueService, 
            IConfiguration configuration, 
            IMediator mediator)
        {
            _accountRepository = accountRepository;
            _emailQueueService = emailQueueService;
            _configuration = configuration;
            _mediator = mediator;
        }

        public async Task<ApiResponse> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.FindByEmailAsync(command.Email);

            if (account == null)
                return ApiResponse.NotFound("Account not found."); //TODO: Message

            await _mediator.Publish(new SendForgotPasswordEmailEvent(account));

            return ApiResponse.Ok(); //TODO
        }
    }
}
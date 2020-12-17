using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityServerTemplate.Core.Repositories;
using IdentityServerTemplate.LanguageResources.Interfaces;
using IdentityServerTemplate.Shared.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;

namespace IdentityServerTemplate.Core.Commands.Handlers
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ApiResponse>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IApiMessagesResource _apiMessagesResource;

        public ResetPasswordCommandHandler(IAccountRepository accountRepository, IApiMessagesResource apiMessagesResource)
        {
            _accountRepository = accountRepository;
            _apiMessagesResource = apiMessagesResource;
        }

        public async Task<ApiResponse> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var tokenDecoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(command.Token));

            var result = await _accountRepository.ResetPassword(command.Email, command.Password, tokenDecoded);

            if (!result.Succeeded)
                return ApiResponse.BadRequest(result.Errors.Select(s => _apiMessagesResource[s.Code]).ToArray());

            return ApiResponse.Ok();
        }
    }
}
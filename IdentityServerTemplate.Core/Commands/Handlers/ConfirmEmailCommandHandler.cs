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
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ApiResponse>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IApiMessagesResource _apiMessagesResource;

        public ConfirmEmailCommandHandler(IAccountRepository accountRepository, IApiMessagesResource apiMessagesResource)
        {
            _accountRepository = accountRepository;
            _apiMessagesResource = apiMessagesResource;
        }

        public async Task<ApiResponse> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken)
        {
            var tokenDecoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(command.Token));

            var result = await _accountRepository.ConfirmEmail(command.Id, tokenDecoded);

            return result.Succeeded 
                ? ApiResponse.Ok() 
                : ApiResponse.BadRequest(result.Errors.Select(s => _apiMessagesResource[s.Code]).ToArray());
        }
    }
}
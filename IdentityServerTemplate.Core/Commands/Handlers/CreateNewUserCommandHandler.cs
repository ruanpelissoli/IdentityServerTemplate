using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IdentityServerTemplate.Core.DTOs;
using IdentityServerTemplate.Core.Entities;
using IdentityServerTemplate.Core.Events;
using IdentityServerTemplate.Core.Repositories;
using IdentityServerTemplate.LanguageResources.Interfaces;
using IdentityServerTemplate.Shared.Infrastructure;
using MediatR;

namespace IdentityServerTemplate.Core.Commands.Handlers
{
    public class CreateNewUserCommandHandler : IRequestHandler<CreateNewUserCommand, ApiResponse>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IApiMessagesResource _apiMessagesResource;

        public CreateNewUserCommandHandler(IAccountRepository accountRepository, IMapper mapper, IMediator mediator, IApiMessagesResource apiMessagesResource)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _mediator = mediator;
            _apiMessagesResource = apiMessagesResource;
        }

        public async Task<ApiResponse> Handle(CreateNewUserCommand request, CancellationToken cancellationToken)
        {
            var account = new Account(request.UserName, request.Email);

            var result = await _accountRepository.CreateUser(account,
                request.Password,
                request.IsDeveloper,
                request.IsPublisher);

            if (!result.Succeeded)
                return ApiResponse.BadRequest(result.Errors.Select(s => _apiMessagesResource[s.Code]).ToArray());

            await _mediator.Publish(new SendConfirmationEmailEvent(account));

            return ApiResponse.Ok(_mapper.Map<AccountDTO>(account));
        }
    }
}
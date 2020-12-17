using System;
using IdentityServerTemplate.Shared.Infrastructure;
using MediatR;

namespace IdentityServerTemplate.Core.Commands
{
    public class ConfirmEmailCommand : IRequest<ApiResponse>
    {
        public Guid Id { get; }
        public string Token { get; }

        public ConfirmEmailCommand(Guid id, string token)
        {
            Id = id;
            Token = token;
        }
    }
}
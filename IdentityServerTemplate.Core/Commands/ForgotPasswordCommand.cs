using IdentityServerTemplate.Shared.Infrastructure;
using MediatR;

namespace IdentityServerTemplate.Core.Commands
{
    public class ForgotPasswordCommand : IRequest<ApiResponse>
    {
        public string Email { get; set; }
    }
}
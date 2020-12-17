using IdentityServerTemplate.Shared.Infrastructure;
using MediatR;

namespace IdentityServerTemplate.Core.Commands
{
    public class ResetPasswordCommand : IRequest<ApiResponse>
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
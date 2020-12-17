using IdentityServerTemplate.Shared.Infrastructure;
using MediatR;

namespace IdentityServerTemplate.Core.Commands
{
    public class CreateNewUserCommand : IRequest<ApiResponse>
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsDeveloper { get; set; }
        public bool IsPublisher { get; set; }
    }
}
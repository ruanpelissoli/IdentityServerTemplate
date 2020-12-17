using IdentityServerTemplate.Core.Entities;
using MediatR;

namespace IdentityServerTemplate.Core.Events
{
    public class SendForgotPasswordEmailEvent : INotification
    {
        public Account Account { get; } //TODO: remove entity

        public SendForgotPasswordEmailEvent(Account account)
        {
            Account = account;
        }
    }
}

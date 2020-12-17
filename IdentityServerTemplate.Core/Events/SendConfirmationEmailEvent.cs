using IdentityServerTemplate.Core.Entities;
using MediatR;

namespace IdentityServerTemplate.Core.Events
{
    public class SendConfirmationEmailEvent : INotification
    {
        public Account Account { get; } //TODO: remove entity

        public SendConfirmationEmailEvent(Account account)
        {
            Account = account;
        }
    }
}
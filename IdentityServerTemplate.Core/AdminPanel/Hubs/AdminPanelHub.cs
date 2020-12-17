using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace IdentityServerTemplate.Core.AdminPanel.Hubs
{
    [AllowAnonymous]
    public class AdminPanelHub : Hub<IAdminPanelClientHub>
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.BroadcastMessage(user, message);
        }
    }
}

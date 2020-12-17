using System.Threading.Tasks;

namespace IdentityServerTemplate.Core.AdminPanel.Hubs
{
    public interface IAdminPanelClientHub
    {
        Task BroadcastMessage(string name, string message);
    }
}

using Microsoft.AspNetCore.SignalR;

namespace bs.Frmwrk.Application.Hubs
{
    public class SecurityHub : Hub
    {
        public async Task NotifyLoginFailed(string user, string ip, DateTime? eventDate)
        {
            await Clients.All.SendAsync("LoginFailed", user, ip, eventDate);
        }

        public async Task NotifyLoginSuccess(string user, string ip, DateTime? eventDate)
        {
            await Clients.All.SendAsync("LoginSuccess", user, ip, eventDate);
        }
    }
}
using Microsoft.AspNetCore.SignalR;

namespace InternProject.Persistence
{
    public class MyHub : Hub
    {
        public async Task SendMessage(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveMessage", message);
        }
    }
}

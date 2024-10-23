using Microsoft.AspNetCore.SignalR;

public class OrderHub : Hub
{
    public async Task JoinOrderGroup(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
    }

    public async Task LeaveOrderGroup(string userId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
    }
}
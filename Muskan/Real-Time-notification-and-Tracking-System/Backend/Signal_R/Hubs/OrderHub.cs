using Microsoft.AspNetCore.SignalR;

namespace Signal_R.Hubs
{
  
        public class OrderHub : Hub
        {
            public async Task NotifyRestaurant(string restaurantId, string message)
            {
                await Clients.User(restaurantId).SendAsync("OrderNotification", message);
            }

            public async Task NotifyUser(string userId, string message)
            {
                await Clients.User(userId).SendAsync("UserNotification", message);
            }
        
    }
}

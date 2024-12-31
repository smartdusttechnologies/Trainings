using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Signal_R.Hubs;
using Signal_R.Models;
using System.Threading.Tasks;

namespace Signal_R.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IHubContext<UserHub> _hubContext;

        public UserController(IHubContext<UserHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder([FromBody] Order order)
        {
            await _hubContext.Clients.Group(order.RestaurantId.ToString()).SendAsync("NewOrderReceived", order.OrderId , order.UserId, order
                .UserLocation.Latitude, order.UserLocation.Longitude);

            return Ok(new { message = "Order placed successfully", orderId = order.OrderId ,userId = order.UserId , order
                .UserLocation.Latitude, order.UserLocation.Longitude });
        }
    }
}

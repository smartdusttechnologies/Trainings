using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Signal_R.Hubs;
using Signal_R.Models;
using System.Threading.Tasks;

namespace Signal_R.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IHubContext<UserHub> _hubContext;

        public RestaurantController(IHubContext<UserHub> hubContext)
        {
            _hubContext = hubContext;
        }

        
        [HttpPost("accept-order")]
        public async Task<IActionResult> AcceptOrder([FromBody] Order order)
        {
            if (order == null)
                return BadRequest(new { message = "Order data is missing." });

           Console.WriteLine($" Order accepted - OrderId: {order.OrderId}, UserId: {order.UserId}");

            try
            {

                Console.WriteLine($"Sending OrderAccepted notification to user: {order.UserId}");
                await _hubContext.Clients.Group(order.UserId).SendAsync("OrderAccepted", order.OrderId ,order.RestaurantId ,order.UserLocation.Latitude ,order.UserLocation.Longitude);
                return Ok(new { message = "Order accepted" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending OrderAccepted notification: {ex.Message}");
                return BadRequest(new { message = "Notification failed" });
            }
        }

        // Decline Order 
        [HttpPost("decline-order")]
        public async Task<IActionResult> DeclineOrder([FromBody] Order order)
        {
            Console.WriteLine($" Order Declined - OrderId: {order.OrderId}, UserId: {order.UserId}");

            try
            {
                await _hubContext.Clients.Group(order.UserId).SendAsync("OrderDeclined", order.OrderId, order.RestaurantId );
                return Ok(new { message = "Order declined" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error sending OrderDeclined notification: {ex.Message}");
                return BadRequest(new { message = "Notification failed" });
            }
        }
        // Confirm Preparation

        [HttpPost("confirm-preparation")]
        public async Task<IActionResult> ConfirmPreparation([FromBody] OrderPreparation orderPreparation)
        {
            if (orderPreparation?.Order == null)
                return BadRequest(new { message = "Order data is missing." });

            try
            {
                var order = orderPreparation.Order;
                var rider = orderPreparation.Rider;
                var restaurantLocation = GetRestaurantLocation(order.RestaurantId);

                Console.WriteLine($"ConfirmPreparation called with OrderId: {order.OrderId}, UserId: {order.UserId}");

                // Notify the user
                await _hubContext.Clients.Group(order.UserId).SendAsync("OrderPrepared", order.RestaurantId, order.OrderId, order.UserId, restaurantLocation.Latitude, restaurantLocation.Longitude);

                // Notify nearby riders
                var nearbyRiders = GetNearbyRiders(order.RestaurantId, 5.0);
                foreach (var nearbyRider in nearbyRiders)
                {
                    await _hubContext.Clients.Group(nearbyRider.RiderId)
                        .SendAsync("NewDeliveryOrder", order.RestaurantId,
               order.OrderId,
               restaurantLocation.Latitude, 
               order.UserId,           
               restaurantLocation.Longitude,
               order.UserLocation.Latitude, order.UserLocation.Longitude);
                }
             


                return Ok(new { message = "Preparation confirmed and notifications sent." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error confirming preparation: {ex.Message}");
                return BadRequest(new { message = "Error confirming preparation", error = ex.Message });
            }
        }

        [HttpPost("accept-delivery")]
        public async Task<IActionResult> AcceptDelivery([FromBody] OrderPreparation orderPreparation)
        {
            if (orderPreparation == null)
            {
                return BadRequest(new { message = "Invalid request payload" });
            }

                Console.WriteLine($"Rider Latitude: {orderPreparation.Rider.Location.Latitude}, Rider Longitude: {orderPreparation.Rider.Location.Longitude}");
            await _hubContext.Clients.Group(orderPreparation.Order.UserId)
            .SendAsync("RiderAssigned", orderPreparation.Rider.RiderId, orderPreparation.Order.OrderId, orderPreparation.Rider.Location.Latitude, orderPreparation.Rider.Location.Longitude);
            Console.WriteLine($"Rider Latitude: {orderPreparation.Rider.Location.Latitude}, Rider Longitude: {orderPreparation.Rider.Location.Longitude}");

            await _hubContext.Clients.Group(orderPreparation.Order.RestaurantId)
                .SendAsync("RiderAssigned", orderPreparation.Rider.RiderId, orderPreparation.Order.OrderId, orderPreparation.Rider.Location.Latitude, orderPreparation.Rider.Location.Longitude);


            return Ok(new { message = "Delivery accepted and notifications sent" });
        }
        [HttpPost("update-location")]
        public async Task<IActionResult> UpdateLocation([FromBody] OrderPreparation order)
        {
           
            if (string.IsNullOrEmpty(order.Rider.RiderId) || string.IsNullOrEmpty(order.Order.OrderId))
            {
                return BadRequest("RiderId and OrderId are required.");
            }
            Console.WriteLine($"UserId: {order.Order.UserId}, RiderId: {order.Rider.RiderId}");


            try
            {
                await _hubContext.Clients.Group(order.Order.UserId)
             .SendAsync("RiderLocationUpdate", order.Rider.RiderId, order.Order.OrderId, order.Rider.Location.Latitude, order.Rider.Location.Longitude);
                await _hubContext.Clients.Group(order.Order.RestaurantId).SendAsync("RiderLocationUpdate", order.Rider.RiderId, order.Order.OrderId, order.Rider.Location.Latitude,order.Rider.Location.Longitude);

                return Ok(new { message = "Rider location updated successfully!" });
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Error updating location: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the location.");
            }
        }


        [HttpPost("delivery-received")]
        public async Task<IActionResult> DeliveryReceived([FromBody] OrderPreparation orderPreparation)
        {
            //await _hubContext.Clients.Group(orderPreparation.Rider.RiderId).SendAsync("UserLocation", orderPreparation.Order.UserId);
            await _hubContext.Clients.Group(orderPreparation.Rider.RiderId).SendAsync("UserLocation", orderPreparation.Order.UserId, orderPreparation.Order.UserLocation.Latitude, orderPreparation.Order.UserLocation.Longitude);

            await _hubContext.Clients.Group(orderPreparation.Order.UserId).SendAsync("DeliveryReceivedByRider", orderPreparation.Order.OrderId, orderPreparation.Rider.Location.Latitude, orderPreparation.Rider.Location.Longitude);

            return Ok(new  { message = "Delivery accept from Restaurant by Rider" });
                }


        [HttpPost("success-delivery")]
        public async Task<IActionResult> OrderDeliverSuccessful([FromBody] OrderPreparation orderPreparation)
        {
            if (orderPreparation == null)
            {
                return BadRequest(new { message = "Invalid request payload" });
            }

            //Console.WriteLine($"Rider Latitude: {orderPreparation.Rider.Location.Latitude}, Rider Longitude: {orderPreparation.Rider.Location.Longitude}");
            await _hubContext.Clients.Group(orderPreparation.Order.UserId)
            .SendAsync("DeliveredSuccessfully", orderPreparation.Rider.RiderId, orderPreparation.Order.OrderId ,orderPreparation.Order.UserId ,orderPreparation.Order.RestaurantId) ;
            //Console.WriteLine($"Rider Latitude: {orderPreparation.Rider.Location.Latitude}, Rider Longitude: {orderPreparation.Rider.Location.Longitude}");

            await _hubContext.Clients.Group(orderPreparation.Order.RestaurantId)
                .SendAsync("DeliveredSuccessfully", orderPreparation.Rider.RiderId, orderPreparation.Order.OrderId, orderPreparation.Order.UserId, orderPreparation.Order.RestaurantId);

            await _hubContext.Clients.Clients(orderPreparation.Rider.RiderId)
                .SendAsync("DeliveredSuccessfully", orderPreparation.Order.RestaurantId, orderPreparation.Order.OrderId, orderPreparation.Order.UserId, orderPreparation.Order.RestaurantId);


            return Ok(new { message = "Delivery accepted and notifications sent" });
        }
        private List<Rider> GetNearbyRiders(string restaurantId, double radiusInKm)
        {
            var allRiders = GetAllRiders();
            var restaurantLocation = GetRestaurantLocation(restaurantId);

            return allRiders.Where(rider =>
                CalculateDistance(restaurantLocation, rider.Location) <= radiusInKm
            ).ToList();
        }

        private List<Rider> GetAllRiders()
        {
            return new List<Rider>
    {
             
        new Rider { RiderId = "Rider1", Location = new Location { Latitude = 25.574192583245868, Longitude = 85.04437131318649 } },
        new Rider { RiderId = "Rider2", Location = new Location { Latitude = 25.579789, Longitude = 85.046345 } },
        new Rider { RiderId = "Rider3", Location = new Location { Latitude = 25.581200, Longitude = 85.046900 } },
        new Rider { RiderId = "Rider4", Location = new Location { Latitude = 25.579100, Longitude = 85.045700 } },
        new Rider { RiderId = "Rider5", Location = new Location { Latitude = 25.600000, Longitude = 85.080000 } },
        new Rider { RiderId = "Rider6", Location = new Location { Latitude = 25.650000, Longitude = 85.120000 } },
        new Rider { RiderId = "Rider7", Location = new Location { Latitude = 25.700000, Longitude = 85.200000 } }
    };
        }


        private Location GetRestaurantLocation(string restaurantId)
        {
            return new Location { Latitude = 25.613949536348287, Longitude = 85.04238958807451 };
        }
    
        private double CalculateDistance(Location location1, Location location2)
        {
            var R = 6371; 
            var dLat = ToRadians(location2.Latitude - location1.Latitude);
            var dLon = ToRadians(location2.Longitude - location1.Longitude);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(location1.Latitude)) * Math.Cos(ToRadians(location2.Latitude)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private double ToRadians(double angle) => angle * Math.PI / 180;

    }
}

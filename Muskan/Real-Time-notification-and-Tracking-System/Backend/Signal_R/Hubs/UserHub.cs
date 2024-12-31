
using Microsoft.AspNetCore.SignalR;
using Signal_R.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Signal_R.Hubs
{
    public class UserHub : Hub
    {
        private static Dictionary<string, string> ConnectedUsers = new Dictionary<string, string>();
        private static Dictionary<string, string> ConnectedRestaurants = new Dictionary<string, string>();
        private static Dictionary<string, (double lat, double lon)> ConnectedRiders = new Dictionary<string, (double lat, double lon)>();
        private static Dictionary<string, string> OrderAssignments = new Dictionary<string, string>();
        public async Task NotifyRestaurant(string restaurantId, string orderId ,string userId , double userLat ,double userLng  )
        {
            Console.WriteLine($"NotifyRestaurant called with restaurantId: {restaurantId}");

            await Clients.Group(restaurantId).SendAsync("NewOrderReceived", orderId , userId, userLat ,userLng) ;
        }

        public async Task NotifyUserOrderAccepted(string userId, string orderId, string restaurantId ,double userLat ,double userLng)
        {
            await Clients.Group(userId).SendAsync("OrderAccepted", orderId, restaurantId, userLat, userLng);
        }

        public async Task NotifyUserOrderDeclined(string userId, string orderId , double userLat, double userLng)
        {
            if (ConnectedUsers.ContainsKey(userId))
            {
                await Clients.Group(ConnectedUsers[userId]).SendAsync("OrderDeclined", orderId, userLat, userLng);
            }
            else
            {
                Console.WriteLine($"User {userId} not connected.");
            }
        }

        public async Task ConfirmPreparation(string userId, string restaurantId, string orderId, double restaurantLat, double restaurantLong, double userLat, double userLng)
        {
            Console.WriteLine($"ConfirmPreparation called with userId: {userId}");
            await Clients.Group(userId).SendAsync("OrderPrepared", restaurantId, orderId, userLat, userLng);

            foreach (var rider in ConnectedRiders)
            {
                var (latitude, longitude) = rider.Value;
                if (CalculateDistance(restaurantLat, restaurantLong, latitude, longitude) <= 5.0)
                {
                    Console.WriteLine($"Notifying rider {rider.Key} about new delivery order {orderId}");
                    await Clients.Group(rider.Key).SendAsync("NewDeliveryOrder", restaurantId, orderId, restaurantLat, restaurantLong, userLat, userLng);
                }
            }

        }
        public async Task AcceptDelivery(string userId, string restaurantId, string riderId, string orderId, double riderLat, double riderLon)
            {
                Console.WriteLine($"AcceptDelivery called with userId: {userId}, restaurantId: {restaurantId}, riderId: {riderId} ");

               
                if (OrderAssignments.ContainsKey(orderId))
                {
                    string alreadyAssignedRider = OrderAssignments[orderId];
                    if (alreadyAssignedRider != riderId)
                    {
                        await Clients.Caller.SendAsync("OrderAlreadyAccepted", "This order has already been accepted by another rider.");
                        return; 
                    }
                }
                else
                {
                  
                    OrderAssignments[orderId] = riderId; 
                }

               
                await Clients.Group(userId).SendAsync("RiderAssigned", riderId, orderId, riderLat, riderLon);
                await Clients.Group(restaurantId).SendAsync("RiderAssigned", riderId, orderId, riderLat, riderLon);
                await Clients.Caller.SendAsync("DeliveryDetails", restaurantId, userId, orderId);

                
            }

        public async Task UpdateRiderLocation(string riderId, string userId, string restaurantId, string orderId, double latitude, double longitude)
        {
           
            if (!OrderAssignments.ContainsKey(orderId) || OrderAssignments[orderId] != riderId)
            {
                await Clients.Caller.SendAsync("Error", "You are not assigned to this order.");
                return;
            }

           
            if (ConnectedRiders.ContainsKey(riderId))
            {
                ConnectedRiders[riderId] = (latitude, longitude);
            }
            else
            {
                Console.WriteLine($"Rider {riderId} is not in the ConnectedRiders list.");
                return;
            }

            Console.WriteLine($"Location updated for rider {riderId}: {latitude}, {longitude}");

            Console.WriteLine($"Location updated for rider {riderId}: {latitude}, {longitude}");

            // Notify the user and restaurant
            await Clients.Group(userId).SendAsync("RiderLocationUpdate", riderId, orderId, latitude, longitude);
            await Clients.Group(restaurantId).SendAsync("RiderLocationUpdate", riderId, orderId, latitude, longitude);
            ////foreach (var (userId, _) in ConnectedUsers)
            ////{
            //await Clients.Group(userId).SendAsync("RiderLocationUpdate", riderId, orderId, latitude, longitude);
            ////}

            ////foreach (var (restaurantId, _) in ConnectedRestaurants)
            ////{
            //    await Clients.Group(restaurantId).SendAsync("RiderLocationUpdate", riderId, orderId, latitude, longitude);
            ////}
        }


        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
            {
                var R = 6371; 
                var dLat = ToRadians(lat2 - lat1);
                var dLon = ToRadians(lon2 - lon1);
                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                return R * c; 
            }

           
            private double ToRadians(double angle) => angle * Math.PI / 180;
    


        public async Task DeliveryReceivedFromRestaurant(string userId, string riderId, double riderLat, double riderLon, double userLat, double userLon)
        {
            await Clients.Group(riderId).SendAsync("UserLocation", userId, userLat, userLon);
            await Clients.Group(userId).SendAsync("DeliveryReceivedByRider", riderId, riderLat, riderLon);


        }


        public async Task OrderDeliver(string userId, string restaurantId, string riderId, string orderId )
        {
            //Console.WriteLine($"AcceptDelivery called with userId: {userId}, restaurantId: {restaurantId}, riderId : {riderId} ");
            await Clients.Group(userId).SendAsync("DeliveredSuccessfully", riderId, orderId );
            await Clients.Group(restaurantId).SendAsync("DeliveredSuccessfully", riderId, orderId);
            await Clients.Caller.SendAsync("DeliveredSuccessfully", restaurantId, userId, orderId);

            //if (ConnectedUsers.ContainsKey(userId))
            //{
            //    var userConnectionId = ConnectedUsers[userId];
            //    await Clients.Group(riderId).SendAsync("UserLocationUpdated", userId, userConnectionId);
            //}
        }
       


        public override async Task OnConnectedAsync() { 
        
       
            string userId = Context.GetHttpContext().Request.Query["userId"];
            string restaurantId = Context.GetHttpContext().Request.Query["restaurantId"];
            string riderId = Context.GetHttpContext().Request.Query["riderId"];

            string riderLatStr = Context.GetHttpContext().Request.Query["riderLat"];
            string riderLngStr = Context.GetHttpContext().Request.Query["riderLng"];

            if (!string.IsNullOrEmpty(riderLatStr) && !string.IsNullOrEmpty(riderLngStr))
            {
                if (double.TryParse(riderLatStr, out double riderLat) && double.TryParse(riderLngStr, out double riderLng))
                {
                    Console.WriteLine($"Rider's location (lat/lng) is connected for rider {riderId} and latitude is  {riderLat} and longitude is {riderLng}");
                    ConnectedRiders[riderId] = (riderLat, riderLng);
                    await Groups.AddToGroupAsync(Context.ConnectionId, riderId);
                }
                else
                {
                    Console.WriteLine($"Invalid latitude/longitude for rider {riderId}: {riderLatStr}, {riderLngStr}");
                }
            }
            else
            {
                Console.WriteLine($"Rider's location (lat/lng) is missing for rider {riderId}");
            }


            if (!string.IsNullOrEmpty(userId))


            {
                Console.WriteLine($"User {userId} connected with connectionId: {Context.ConnectionId}");

                ConnectedUsers[userId] = Context.ConnectionId;
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }

            if (!string.IsNullOrEmpty(restaurantId))
            {
                Console.WriteLine($"Restaurant {restaurantId} connected with connectionId: {Context.ConnectionId}");


                ConnectedRestaurants[restaurantId] = Context.ConnectionId;
                await Groups.AddToGroupAsync(Context.ConnectionId, restaurantId);
            }

            await base.OnConnectedAsync();
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string userId = GetUserIdFromConnectionId(Context.ConnectionId);
            string restaurantId = GetRestaurantIdFromConnectionId(Context.ConnectionId);
            string riderId = GetRiderIdFromConnectionId(Context.ConnectionId);

            if (userId != null)
            {
                ConnectedUsers.Remove(userId);
            }

            if (restaurantId != null)
            {
                ConnectedRestaurants.Remove(restaurantId);
            }

            if (riderId != null)
            {
                ConnectedRiders.Remove(riderId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        private string GetUserIdFromConnectionId(string connectionId) =>
            ConnectedUsers.FirstOrDefault(x => x.Value == connectionId).Key;

        private string GetRestaurantIdFromConnectionId(string connectionId) =>
            ConnectedRestaurants.FirstOrDefault(x => x.Value == connectionId).Key;

        private string GetRiderIdFromConnectionId(string connectionId) =>
            ConnectedRiders.FirstOrDefault(x => x.Key == connectionId).Key;



        //public async Task AcceptDelivery(string userId, string restaurantId, string riderId, string orderId, double riderLat, double riderLon)
        //{
        //    Console.WriteLine($"AcceptDelivery called with userId: {userId}, restaurantId: {restaurantId}, riderId : {riderId} ");
        //    await Clients.Group(userId).SendAsync("RiderAssigned", riderId, orderId, riderLat, riderLon);
        //    await Clients.Group(restaurantId).SendAsync("RiderAssigned", riderId, orderId, riderLat, riderLon);
        //    await Clients.Caller.SendAsync("DeliveryDetails", restaurantId, userId, orderId);

        //    //if (ConnectedUsers.ContainsKey(userId))
        //    //{
        //    //    var userConnectionId = ConnectedUsers[userId];
        //    //    await Clients.Group(riderId).SendAsync("UserLocationUpdated", userId, userConnectionId);
        //    //}
        //}



        // Notify all riders within 5 km of the restaurant
        //public async Task ConfirmPreparation(string userId, string restaurantId, string orderId, double restaurantLat, double restaurantLong, double userLat, double userLng)
        //{
        //    Console.WriteLine($"ConfirmPreparation called with userId: {userId}");
        //    await Clients.Group(userId).SendAsync("OrderPrepared", restaurantId, orderId, userLat, userLng);

        //    List<string> notifiedRiders = new List<string>();

        //    foreach (var rider in ConnectedRiders)
        //    {
        //        var (latitude, longitude) = rider.Value;
        //        if (CalculateDistance(restaurantLat, restaurantLong, latitude, longitude) <= 5.0)
        //        {
        //            Console.WriteLine($"Notifying rider {rider.Key} about new delivery order {orderId}");
        //            await Clients.Group(rider.Key).SendAsync("NewDeliveryOrder", restaurantId, orderId, restaurantLat, restaurantLong, userLat, userLng);
        //            notifiedRiders.Add(rider.Key); 
        //        }
        //    }

        //               }

    }
}

namespace Signal_R.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        //public (double lat, double lon) UserLocation { get; set; }
        public Location UserLocation { get; set; }
        public string RestaurantId { get; set; }
        //public string UserAddress { get; set; }
        //public (double lat ,double lon ) Restaurantlocation { get; set; }
    }


}

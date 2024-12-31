import React, { useState, useEffect, useRef } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";
import axios from "axios";
import {
  GoogleMap,
  LoadScript,
  DirectionsService,
  useJsApiLoader,
  Polyline,
  DirectionsRenderer,
  Marker,
} from "@react-google-maps/api";
import HandleRiderButton from "./handleRiderButton";
import RiderMap from "./RiderMap";
const RiderNotification = () => {
  const [isTracking, setIsTracking] = useState(false); 
  const [trackingInterval, setTrackingInterval] = useState(null);
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [riderLat, setRiderLat] = useState(null);
  const [riderLng, setRiderLng] = useState(null);
  // const [riderLat, setRiderLat] = useState(25.607196371638086);
  // const [riderLng, setRiderLng] = useState(85.16396783073729);
  const [userLat, setUserLat] = useState(null);
  const [userLng, setUserLng] = useState(null);
  const [distance, setDistance] = useState(null);
  const [duration, setDuration] = useState(null);
  const [restaurantLat, setRestaurantLat] = useState(null);
  const [restaurantLng, setRestaurantLng] = useState(null);
  const [directionsResponse, setDirectionsResponse] = useState(null);
  const [directionsError, setDirectionsError] = useState(null);
  const [orderId, setOrdersId] = useState(null);
  const [restaurantId, setRestaurantId] = useState(null);
  const [userId, setUserId] = useState(null);
  const pathIndexRef = useRef(0);
  const [currentLocationIndex, setCurrentLocationIndex] = useState(0);
  const [currentPosition, setCurrentPosition] = useState(null);
  const [isOrderAccepted, setIsOrderAccepted] = useState(false);

  const riderId = "Rider1";

  const apiKey = process.env.REACT_APP_GOOGLE_MAPS_API_KEY;

  const { isLoaded, loadError } = useJsApiLoader({
    googleMapsApiKey: apiKey,
  });

  const riderIcon = isLoaded
    ? {
      url: "/Images/rider.png",
      scaledSize: new window.google.maps.Size(40, 40),
    }
    : null;

  const restaurantIcon = isLoaded
    ? {
      url: "/Images/restaurant.png",
      scaledSize: new window.google.maps.Size(40, 40),
    }
    : null;
  const userIcon = isLoaded
    ? {
      url: "/Images/Home.png",
      scaledSize: new window.google.maps.Size(40, 40),
    }
    : null;
  const route = [
    {
      location: "Rajendra Nagar",
      lat: 25.607196371638086,
      lng: 85.16396783073729,
    },

    { location: "Patna Jn", lat: 25.604108921274833, lng: 85.13705090607819 },

    {
      location: "Sanjiv Gandhi Park",
      lat: 25.606389719575212,
      lng: 85.0988639480713,
    },

    { location: "Bailey Road", lat: 25.60597775756284, lng: 85.08458302539263 },

     {
      location: "Tasty Kitchen",
      lat: 25.614605699639768,
      lng: 85.04209056402705,
    },
  ];

  useEffect(() => {
    if (riderLat !== null && riderLng !== null) {
      const connection = new HubConnectionBuilder()
        .withUrl(
          `https://localhost:7076/userHub?riderId=${riderId}&riderLat=${riderLat}&riderLng=${riderLng}`
        )
        .build();

      const startConnection = async () => {
        try {
          await connection.start();
          console.log("Rider connected to SignalR");
          connection.on(
            "NewDeliveryOrder",
            (
              restaurantId,
              orderId,
              restaurantLat,
              userId,
              restaurantLng,
              userLat,
              userLng
            ) => {
              setOrdersId(orderId);
              setRestaurantId(restaurantId);
              setUserId(userId);
              console.log("Received data:", {
                restaurantId,
                orderId,
                restaurantLat,
                userId,
                restaurantLng,
                userLat,
                userLng,
              });
              setOrders((prevOrders) => [
                ...prevOrders,
                {
                  restaurantId,
                  orderId,
                  restaurantLat,
                  userId,
                  restaurantLng,
                  userLat,
                  userLng,
                  status: "New Delivery Order",
                },
              ]);
              setRestaurantLat(restaurantLat);
              console.log("restaurant", restaurantLat);
              setRestaurantLng(restaurantLng);
              console.log("restaurant", restaurantLng);
              setUserLat(userLat);
              console.log("user lat", userLat);
              setUserLng(userLng);
              console.log("user lng ", userLng);
            }
          );
          connection.on("UserLocation", (userId, userLat, userLng) => {
            if (userLat && userLng) {
              console.log(
                `User Location Updated: ${userId} - Latitude: ${userLat}, Longitude: ${userLng}`
              );
              setOrders((prevOrders) =>
                prevOrders.map((order) =>
                  order.userId === userId
                    ? {
                      ...order,
                      status: `User location ${userId} - Latitude:  ${userLat}, Longitude: ${userLng}`,
                      userLat,
                      userLng,
                    }
                    : order
                )
              );
            } else {
              console.warn("Received invalid location data from server.");
            }
          });

          connection.on("OrderPrepared", (restaurantId, orderId) => {
            setOrders((prevOrders) =>
              prevOrders.map((order) =>
                order.orderId === orderId
                  ? { ...order, status: "Order Prepared" }
                  : order
              )
            );
          });

          connection.on("RiderAssigned", (riderId, orderId) => {
            setOrders((prevOrders) =>
              prevOrders.map((order) =>
                order.orderId === orderId
                  ? { ...order, status: `Rider ${riderId} Assigned` }
                  : order
              )
            );
          });
          setLoading(false);
        } catch (err) {
          console.error("Error starting SignalR connection:", err);
          setError("Failed to connect to SignalR");
        }
      };
      startConnection();
      return () => {
        connection.stop().then(() => console.log("Connection stopped"));
      };
    }
  }, [riderLat, riderLng, riderId]);

  useEffect(() => {
    const path = [
      { lat: 25.607196371638086, lng: 85.16396783073729 }, //Rajendra nagar
      { lat: 25.604108921274833, lng: 85.13705090607819 },
      { lat: 25.606389719575212, lng: 85.0988639480713 },
      { lat: 25.60597775756284, lng: 85.08458302539263 },
      { lat: restaurantLat, lng: restaurantLng }, //Tasty Kitchen

      { lat: 25.583396773981878, lng: 85.04366918167764 }, //Danapur Station
      { lat: 25.580081212251873, lng: 85.05202100474972 }, //badi badalpura
      { lat: 25.578162960643127, lng: 85.04504906775514 }, //moti Chowk
      { lat: 25.575095104796205, lng: 85.04330374185854 }, //Jyoti Library
      { lat: userLat, lng: userLng },
    ];

    let previousLocation = { lat: riderLat, lng: riderLng };

    const simulateMovement = () => {
      const currentIndex = pathIndexRef.current;
      const newLocation = path[currentIndex];

      console.log(
        `Previous Location: Lat: ${previousLocation.lat}, Lng: ${previousLocation.lng}`
      );
      console.log(
        `Current Location: Lat: ${newLocation.lat}, Lng: ${newLocation.lng}`
      );

      const moved =
        previousLocation.lat !== newLocation.lat ||
        previousLocation.lng !== newLocation.lng;

      console.log(moved ? "The rider moved!" : "No movement detected.");

      setRiderLat(newLocation.lat);
      setRiderLng(newLocation.lng);

      previousLocation = { lat: newLocation.lat, lng: newLocation.lng };
 
      axios
        .post("https://localhost:7076/api/restaurant/update-location", {
          order: {
            orderId: orderId,
            userId: userId,
            userLocation: {
              latitude: newLocation.lat,
              longitude: newLocation.lng,
            },
            restaurantId: restaurantId,
          },
          rider: {
            riderId: riderId,
            location: {
              latitude: newLocation.lat,
              longitude: newLocation.lng,
            },
          },
        })
        .then((response) => {
          console.log("Location updated successfully:", response.data);
        })
        .catch((error) => {
          console.error("Error updating location:", error);
        });

   

      if (currentIndex === 4) {
        axios.post("https://localhost:7076/api/restaurant/delivery-received", {
          order: {
            orderId: orderId,
            userId: userId,
            userLocation: {
              latitude: newLocation.lat,
              longitude: newLocation.lng,
            },
            restaurantId: restaurantId,
          },
          rider: {
            riderId: riderId,
            location: {
              latitude: newLocation.lat,
              longitude: newLocation.lng,
            },
          },
        });
        console.log("Message sent to the restaurant: Order picked up");
      }
      if (currentIndex === path.length - 1) {
        axios.post("https://localhost:7076/api/restaurant/success-delivery", {
          order: {
            orderId: orderId,
            userId: userId,
            userLocation: {
              latitude: newLocation.lat,
              longitude: newLocation.lng,
            },
            restaurantId: restaurantId,
          },
          rider: {
            riderId: riderId,
            location: {
              latitude: newLocation.lat,
              longitude: newLocation.lng,
            },
          },
        });
        console.log("Message sent to the User: They reached the destination");
      }
      // };
      if (currentIndex < path.length - 1) {
        pathIndexRef.current++;
       
        console.log("Current location index: " + currentIndex);
      } else {
        pathIndexRef.current = 0;
      }
    };

    const locationInterval = setInterval(simulateMovement, 15000);

    return () => clearInterval(locationInterval);
  }, [riderId, orderId, userId, restaurantId]);

  const handleApiError = (error, message) => {
    console.error(message, error);
    if (error.response) {
      console.error("Response Error:", error.response);
    } else if (error.request) {
      console.error("Request Error:", error.request);
    } else {
      console.error("Error:", error.message);
    }
  };

  useEffect(() => {
    if (
      riderLat &&
      riderLng &&
      restaurantLat &&
      restaurantLng &&
      userLat &&
      userLng
    ) {
      if (window.google) {
        const directionsService = new window.google.maps.DirectionsService();
        
        directionsService.route(
          {
            // origin: { lat: riderLat, lng: riderLng },
            // destination: { lat: userLat, lng: userLng },
            origin: { lat: userLat, lng: userLng },
            destination: { lat: riderLat, lng: riderLng },
            waypoints: [
              { location: { lat: restaurantLat, lng: restaurantLng } },
            ],
            travelMode: window.google.maps.TravelMode.DRIVING,
          },
          (result, status) => {
            if (status === "OK") {
              setDirectionsResponse(result);
              const leg = result.routes[0].legs[0];
              setDistance(leg.distance.text);
              setDuration(leg.duration.text);
            } else {
              const errorMessage = `Error fetching directions: ${status}`;
              setDirectionsError(errorMessage);
              console.error(errorMessage);
            }
          }
        );

      }
    }
  }, [riderLat, riderLng, restaurantLat, restaurantLng, userLat, userLng]);
  const handleAcceptOrder = async (
    orderId,
    userId,
    restaurantId,
    userLat,
    userLng
  ) => {
    console.log(userId);
    console.log(restaurantId);
    if (!userId || !restaurantId) {
      alert("Missing required data: userId or restaurantId");
      return;
    }
    try {
      const orderData = {
        order: {
          orderId: orderId,
          userId: userId,
          userLocation: {
            latitude: userLat,
            longitude: userLng,
          },

          restaurantId: restaurantId,
          restaurantlocation: {
            latitude: restaurantLat,
            longitude: restaurantLng,
          },
        },
        rider: {
          riderId: riderId,
          location: {
            // lat: 25.6049152,
            // lon: 85.1410944,
            latitude: riderLat,
            longitude: riderLng,
          },
        },
      };
      console.log("Sending Rider's Location:", riderLat, riderLng);

      console.log("Sending Order Data:", orderData);
      console.log("Sending Order Data:", JSON.stringify(orderData, null, 2));

      const response = await axios.post(
        "https://localhost:7076/api/restaurant/accept-delivery",
        orderData
      );
      setIsOrderAccepted(true);
      console.log("Order accepted:", response.data);
      // setUserLat(userLat);
      // setUserLng(userLng);
      alert(`Order ${orderId} accepted`);
    } catch (error) {
      console.error("Error accepting order:", error);
      alert("Error: Failed to accept the order.");
    }
  };

  const handleDeclineOrder = (orderId) => {
    setOrders((prevOrders) =>
      prevOrders.filter((order) => order.orderId !== orderId)
    );
    setIsOrderAccepted(false);
    alert(`Order ${orderId} declined`);
  };
  const handleDeliveryAcceptFromRestaurant = async (
    orderId,
    userId,
    restaurantId,
    userLat,
    userLng
  ) => {
    if (!userId || !restaurantId) {
      alert("Missing required data: userId or restaurantId");
      return;
    }

    try {
      const orderData = {
        order: {
          orderId: orderId,
          userId: userId,
          userLocation: {
            latitude: userLat,
            longitude: userLng,
          },

          restaurantId: restaurantId,
          restaurantlocation: {
            latitude: restaurantLat,
            longitude: restaurantLng,
          },
        },
        rider: {
          riderId: riderId,
          location: {
            latitude: riderLat,
            longitude: riderLng,
          },
        },
      };

      console.log("Sending Order Data:", orderData);
      const response = await axios.post(
        "https://localhost:7076/api/restaurant/delivery-received",
        orderData
      );
      console.log("API Response:", response.data);
      alert("Order received successfully!");
    } catch (error) {
      console.error(
        "Error while accepting order:",
        error.response?.data || error.message
      );
      alert("Failed to accept the order.");
    }
  };

  const handleSuccessDelivery = async (
    orderId,
    userId,
    restaurantId,
    userLat,
    userLng
  ) => {
    if (!userId || !restaurantId) {
      alert("Missing required data: userId or restaurantId");
      return;
    }

    try {
      const orderData = {
        order: {
          orderId: orderId,
          userId: userId,
          userLocation: {
            latitude: userLat,
            longitude: userLng,
          },

          restaurantId: restaurantId,
          restaurantlocation: {
            latitude: restaurantLat,
            longitude: restaurantLng,
          },
        },
        rider: {
          riderId: riderId,
          location: {
            latitude: riderLat,
            longitude: riderLng,
          },
        },
      };

      console.log(" Order Success data send :", orderData);
      const response = await axios.post(
        "https://localhost:7076/api/restaurant/success-delivery",
        orderData
      );
      console.log("API Response:", response.data);
      alert("Order  successfully delivered to User!");
    } catch (error) {
      console.error(
        "Error while successing order:",
        error.response?.data || error.message
      );
      alert("Failed to accept the order.");
    }
  };

  if (!isLoaded) return <div>Loading Google Maps...</div>;
  if (loadError) return <div>Error loading Google Maps</div>;

  return (
    <div className="container mt-4">
      <h1 className="mb-4">Rider Notifications</h1>
      {loading ? (
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
      ) : error ? (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      ) : (
        <div>
          {orders.map((order, index) => (
            <div key={index} className="card m-2">
              <div className="card-body">
                <div className="d-flex justify-content-between">
                  <HandleRiderButton
                    order={order}
                    handleAcceptOrder={handleAcceptOrder}
                    handleDeclineOrder={handleDeclineOrder}
                    handleDeliveryAcceptFromRestaurant={
                      handleDeliveryAcceptFromRestaurant
                    }
                    handleSuccessDelivery={handleSuccessDelivery}
                  />
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
      <div>
        <RiderMap
          riderLat={riderLat}
          riderLng={riderLng}
          userLat={userLat}
          userLng={userLng}
          restaurantLat={restaurantLat}
          restaurantLng={restaurantLng}
          directionsResponse={directionsResponse}
          directionsError={directionsError}
          distance={distance}
          duration={duration}
        />
      </div>
    </div>
  );
};

export default RiderNotification;

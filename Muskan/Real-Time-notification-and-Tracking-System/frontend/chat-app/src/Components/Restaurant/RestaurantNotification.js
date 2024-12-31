import React, { useState, useEffect, useRef } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";
import axios from "axios";
import {
  GoogleMap,
  DirectionsService,
  Polyline,
  useJsApiLoader,
  DirectionsRenderer,
  Marker,
} from "@react-google-maps/api";
import RestaurantMap from "./RestaurantMap";
import HandleRestaurantButton from "./handleRestaurantButton";

const restaurantId = "789";

const RestaurantNotification = () => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [restaurantLat, setRestaurantLat] = useState(25.613949536348287);
  const [restaurantLng, setRestaurantLng] = useState(85.04238958807451);
  const [riderLat, setRiderLat] = useState(null);
  const [riderLng, setRiderLng] = useState(null);
  const [userLat, setUserLat] = useState(null);
  const [userLng, setUserLng] = useState(null);
  const connectionRef = useRef(null);
  const [directionsResponse, setDirectionsResponse] = useState(null);
  const [directionsError, setDirectionsError] = useState(null);
  const [distance, setDistance] = useState(null);
  const [duration, setDuration] = useState(null);
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
  useEffect(() => {
    const connection = new HubConnectionBuilder()
      .withUrl(`https://localhost:7076/userHub?restaurantId=${restaurantId}`)
      .build();

    connection
      .start()
      .then(() => {
        console.log("Restaurant connected to SignalR");
        connectionRef.current = connection;
      })
      .catch((err) => console.error("SignalR connection failed:", err));
    connection.onclose(() => {
      console.log("SignalR connection closed");
    });

    connection.onreconnecting(() => {
      console.log("SignalR connection reconnecting...");
    });

    connection.onreconnected(() => {
      console.log("SignalR connection reconnected");
    });

    connection.on("NewOrderReceived", (orderId, userId, userLat, userLng) => {
      setUserLat(userLat);
      setUserLng(userLng);
      console.log("New order received:", orderId, userId, userLat, userLng);
      setOrders((prevOrders) => [
        ...prevOrders,
        { orderId, userId, userLat, userLng, status: "New Order" },
      ]);
    });

    connection.on("RiderAssigned", (riderId, orderId, riderLat, riderLng) => {
      console.log(
        `Received Rider Location: Lat: ${riderLat}, Lon: ${riderLng}`
      );
      setRiderLat(riderLat);
      setRiderLng(riderLng);

      setOrders((prevOrders) =>
        prevOrders.map((order) =>
          order.orderId === orderId
            ? {
              ...order,
              status: `Rider ${riderId} Assigned`,
              riderLat,
              riderLng,
            }
            : order
        )
      );
    });
    connection.on(
      "DeliveredSuccessfully",
      (riderId, orderId, restaurantId, userId) => {
        setOrders((prevOrders) =>
          prevOrders.map((order) =>
            order.orderId === orderId
              ? {
                ...order,
                status: `Rider ${riderId} Successfully delivered order ${orderId} to the user  ${userId}  by the restaurant ${restaurantId}`,
              }
              : order
          )
        );
      }
    );
    connection.on("OrderPrepared", (orderId) => {
      console.log(`Order ${orderId} is now being prepared.`);
    });

    connection.on(
      "RiderLocationUpdate",
      (riderId, orderId, latitude, longitude) => {
        console.log(
          `Rider Location Updated - RiderId: ${riderId}, OrderId: ${orderId}, Latitude: ${latitude}, Longitude: ${longitude}`
        );
        setRiderLat(latitude);
        setRiderLng(longitude);
        setOrders((prevOrders) =>
          prevOrders.map((order) =>
            order.orderId === orderId
              ? {
                ...order,
                status: `Update rider loation   ${riderId}  for order ${orderId} latitude ${latitude} and longitude ${longitude} `,
              }
              : order
          )
        );
      }
    );

    return () => connection.stop();
  }, []);
  useEffect(() => {
    if (riderLat && riderLng && restaurantLat && restaurantLng) {
      if (window.google) {
        const directionsService = new window.google.maps.DirectionsService();
        directionsService.route(
          {
            destination: { lat: restaurantLat, lng: restaurantLng },
            origin: { lat: riderLat, lng: riderLng },
            travelMode: window.google.maps.TravelMode.DRIVING,
          },
          (result, status) => {
            if (status === "OK") {
              setDirectionsResponse(result);
              console.log("Directions Request:", {
                destination: { lat: restaurantLat, lng: restaurantLng },
                origin: { lat: riderLat, lng: riderLng },
              });
              const leg = result.routes[0].legs[0];

              setDistance(leg.distance.text);
              setDuration(leg.duration.text);
              console.log("Rider Location:", riderLat, riderLng);
            } else {
              setDirectionsError(`Error fetching directions: ${status}`);
            }
          }
        );
      }
    }
  }, [riderLat, riderLng, restaurantLat, restaurantLng]);

  const handleAcceptOrder = async (orderId, userId, userLat, userLng) => {
    console.log("Accept Order Debug Payload:", {
      orderId,
      restaurantId,
      userId,
      userLat,
      userLng,
    });

    try {
      await axios.post("https://localhost:7076/api/restaurant/accept-order", {
        orderId,
        restaurantId,
        userId,
        userLocation: {
          latitude: userLat,
          longitude: userLng,
        },
      });
      console.log(
        `Order ${orderId} accepted for user ${userId} and latitude of user ${userLat} and longitude of user ${userLng}`
      );
      alert(`Order ${orderId} has been accepted successfully.`);
    } catch (err) {
      console.error("Request Error:", err.response?.data);
      alert("Order acceptance failed. Please try again.");
    }
  };
  const handleDeclineOrder = async (orderId, userId, userLat, userLng) => {
    console.log("Sending Decline Order Payload", {
      orderId,
      restaurantId,
      userId,
    });

    try {
      await axios.post("https://localhost:7076/api/restaurant/decline-order", {
        orderId,
        restaurantId,
        userId,
        userLocation: {
          latitude: userLat,
          longitude: userLng,
        },
      });

      console.log(`Order ${orderId} declined successfully`);
      setOrders((prevOrders) =>
        prevOrders.filter((order) => order.orderId !== orderId)
      );
      alert(`Order ${orderId} declined`);
    } catch (err) {
      console.error("Decline Order Request Failed", err.response?.data);
    }
  };

  const confirmOrderPreparation = async (orderId, userId, userLat, userLng) => {
    console.log("Confirm Preparation Payload:", {
      orderId,
      userId,
      userLat,
      userLng,
    });
    try {
      const orderData = {
        Order: {
          orderId,
          restaurantId,
          userId,
          userLocation: {
            latitude: userLat,
            longitude: userLng,
          },
        },
        Rider: {
          RiderId: "Rider1",
          Location: {
            latitude: 25.574,
            longitude: 85.044,
          },
        },
        RestaurantLat: restaurantLat,
        RestaurantLong: restaurantLng,
      };

      console.log("Order Data", orderData);

      const response = await axios.post(
        "https://localhost:7076/api/restaurant/confirm-preparation",
        orderData
      );

      if (response.data && response.data.message) {
        alert(response.data.message);
      }

      console.log(`Order ${orderId} preparation confirmed.`);
    } catch (err) {
      console.error("Confirm preparation request failed", err.response?.data);
    }
  };

  if (!isLoaded) return <div>Loading Google Maps...</div>;
  if (loadError) return <div>Error loading Google Maps</div>;

  return (
    <div className="container mt-4">
      {restaurantLat !== null && restaurantLng !== null ? (
        <div className="alert alert-info" role="alert">
          <strong>Current Location:</strong> Lat: {restaurantLat}, Lng:{" "}
          {restaurantLng}
        </div>
      ) : (
        <div className="alert alert-warning" role="alert">
          Fetching your location...
        </div>
      )}
      <h2>Incoming Orders</h2>
      {orders.length > 0 ? (
        orders.map((order) => (
          <div key={order.orderId} className="card mb-3">
            <div className="card-body">
              <HandleRestaurantButton
                order={order}
                handleAcceptOrder={handleAcceptOrder}
                handleDeclineOrder={handleDeclineOrder}
                confirmOrderPreparation={confirmOrderPreparation}
                riderLat={riderLat}
                riderLng={riderLng}
                userLat={userLat}
                userLng={userLng}
                restaurantLat={restaurantLat}
                restaurantLng={restaurantLng}
              />
            </div>
          </div>
        ))
      ) : (
        <p>No orders received yet</p>
      )}
      {distance && duration && (
        <div className="alert alert-info">
          <p>
            <strong>Distance:</strong> {distance}
          </p>
          <p>
            <strong>Duration:</strong> {duration}
          </p>
        </div>
      )}

      <GoogleMap
        mapContainerStyle={{ height: "400px", width: "100%" }}
        center={{ lat: restaurantLat || 0, lng: restaurantLng || 0 }}
        zoom={14}
      >
        <Marker position={{ lat: riderLat, lng: riderLng }} icon={riderIcon} />

        <Marker position={{ lat: userLat, lng: userLng }} icon={userIcon} />

        <Marker
          position={{ lat: restaurantLat, lng: restaurantLng }}
          icon={restaurantIcon}
        />
        {directionsResponse && (
          <DirectionsRenderer directions={directionsResponse} />
        )}
        {directionsError && (
          <div className="alert alert-danger">{directionsError}</div>
        )}
      </GoogleMap>
    </div>
  );
};

export default RestaurantNotification;

import React, { useState, useEffect } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { FaBell } from "react-icons/fa";
import {
  GoogleMap,
  LoadScript,
  DirectionsService,
  useJsApiLoader,
  Polyline,
  DirectionsRenderer,
  Marker,
} from "@react-google-maps/api";
import HandleUserButton from "./HandleUserButton";
const NotificationUser = ({ userId }) => {
  const [notifications, setNotifications] = useState([]);
  const [userLat, setUserLat] = useState(25.575215031858086);
  const [userLng, setUserLng] = useState(85.04438692975793);
  // 25.575215031858086, 85.04438692975793
  const [error, setError] = useState(null);
  const [distance, setDistance] = useState(null);
  const [duration, setDuration] = useState(null);
  const [restaurantLat, setRestaurantLat] = useState(null);
  const [restaurantLng, setRestaurantLng] = useState(null);
  const [directionsResponse, setDirectionsResponse] = useState(null);
  const [directionsError, setDirectionsError] = useState(null);
  const [riderLat, setRiderLat] = useState(null);
  const [riderLng, setRiderLng] = useState(null);
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
      .withUrl(`https://localhost:7076/userHub?userId=${userId}`)
      .withAutomaticReconnect()
      .build();

    connection
      .start()
      .then(() => console.log("User connected to SignalR"))
      .catch((err) => console.error("Connection Error:", err));

    connection.on("OrderAccepted", (orderId) => {
      setNotifications((prev) => [
        ...prev,
        {
          id: orderId,
          message: `Order ${orderId} has been accepted.`,
          type: "accepted",
        },
      ]);
    });

    connection.on("OrderDeclined", (orderId) => {
      setNotifications((prev) => [
        ...prev,
        {
          id: orderId,
          message: `Order ${orderId} was declined.`,
          type: "declined",
        },
      ]);
    });

    connection.on("OrderPrepared", (restaurantId, orderId, userId, restaurantLat, restaurantLng) => {
      setRestaurantLat(restaurantLat);
      setRestaurantLng(restaurantLng);
      console.log(restaurantLat)
      console.log(restaurantLng)

      setNotifications((prev) => [
        ...prev,
        {
          id: `prepared-${Date.now()}`,
          message: "Your order is prepared.",
          type: "prepared",
        },
      ]);
    });

    connection.on(
      "RiderAssigned",
      (riderId, orderId, riderLat, riderLon, restaurantLat, restaurantLng) => {
        // setRiderLat(riderLat);
        // // setRiderLng(riderLon);

        // console.log(restaurantLat, restaurantLng);
        setNotifications((prev) => [
          ...prev,
          {
            id: orderId,
            message: `Rider ${riderId} has been assigned to your order ${orderId}.`,
            type: "assigned",
          },
        ]);
      }
    );
    connection.on("DeliveredSuccessfully", (riderId, orderId) => {
      setNotifications((prev) => [
        ...prev,
        {
          id: orderId,
          message: `Rider ${riderId} has been successfully deliver the order ${orderId}.`,
          type: "delivered",
        },
      ]);
    });

    connection.on("DeliveryReceivedByRider", (riderId, riderLat, riderLon) => {
      // Compare riderId with the userId (or the correct identifier you're tracking)
      // if (riderId === userId) {
      // setRiderLat(riderLat);
      // setRiderLng(riderLon);

      setNotifications((prev) => [
        ...prev,
        {
          riderId: riderId,
          message: `Rider ${riderId} has accepted the order from the restaurant , and their current location is latitude: ${riderLat} and longitude: ${riderLon}`,
          type: "received",
        },
      ]);
      console.log(
        `Delivery received by rider ${riderId}. Location: (${riderLat}, ${riderLon})`
      );
      // }
    });
    connection.on(
      "RiderLocationUpdate",
      (riderId, orderId, latitude, longitude) => {
        console.log(
          `Rider ${riderId} location updated: Lat: ${latitude}, Lon: ${longitude} and order Id is ${orderId}.`
        );
        setRiderLat(latitude);
        setRiderLng(longitude);
        setNotifications((prev) => [
          ...prev,
          {
            message: `Rider ${riderId} location updated: Lat: ${latitude}, Lon: ${longitude} and order Id is ${orderId}.`,
            type: "UpdateLocation",
          },
        ]);
      }
    );
    return () => {
      connection
        .stop()
        .catch((err) =>
          console.error("Error stopping SignalR connection:", err)
        );
    };
  }, [userId]);

  // console.log(`Rider location : lat ${riderLat} and lon ${riderLat}`)
  useEffect(() => {
    if (riderLat && riderLng && userLat && userLng) {
      if (window.google) {
        const directionsService = new window.google.maps.DirectionsService();
        directionsService.route(
          {
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
              console.log("Directions Request:", {
                destination: { lat: userLat, lng: userLng },
                origin: { lat: riderLat, lng: riderLng },
                waypoints: [
                  { location: { lat: restaurantLat, lng: restaurantLng } },
                ],
              });
              const leg = result.routes[0].legs[0];

              setDistance(leg.distance.text);
              setDuration(leg.duration.text);
              setDirectionsError(`Error fetching directions: ${status}`);
              console.log("Directions Request Error:", {
                origin: { lat: userLat, lng: userLng },
                destination: { lat: riderLat, lng: riderLng },
              });
            }
          }
        );
      }
    }
  }, [userLat, userLng, riderLat, riderLng]);

  const handleCloseNotification = (id) => {
    setNotifications((prev) =>
      prev.filter((notification) => notification.id !== id)
    );
  };
  if (!isLoaded) return <div>Loading Google Maps...</div>;
  if (loadError) return <div>Error loading Google Maps</div>;

  return (
    <div className="container mt-4">
      {userLat !== null && userLng !== null ? (
        <div className="alert alert-info" role="alert">
          <strong>Current Location:</strong> Lat: {userLat}, Lng: {userLng}
        </div>
      ) : (
        <div className="alert alert-warning" role="alert">
          Fetching your location...
        </div>
      )}

      <div style={{ top: 10, right: 20, zIndex: 1000 }}>
        {notifications.map((notification) => (
          <div
            key={notification.id}
            style={{
              position: "relative",
              marginBottom: "10px",
              backgroundColor:
                notification.type === "accepted"
                  ? "green"
                  : notification.type === "declined"
                    ? "red"
                    : notification.type === "prepared"
                      ? "blue"
                      : notification.type === "UpdateLocation"
                        ? "Pink"
                        : "orange",
              padding: "10px",
              color: notification.type === "UpdateLocation" ? "black" : "white",
              // borderRadius: "50%",
              display: "flex",
              alignItems: "center",
              cursor: "pointer",
            }}
            onClick={() => handleCloseNotification(notification.id)}
          >
            {" "}
            <FaBell size={24} style={{ marginRight: "8px" }} />
            {notification.message}
            <button
              onClick={() => handleCloseNotification(notification.id)}
              style={{
                color: notification.type === "UpdateLocation" ? "Red" : "white",
                position: "absolute",
                top: "2px",
                right: "2px",
                background: "none",
                border: "none",
                // color: "white",
                fontSize: "14px",
                cursor: "pointer",
              }}
            >
              ×
            </button>
          </div>
        ))}
      
        <GoogleMap
          mapContainerStyle={{ height: "400px", width: "100%" }}
          center={{ lat: userLat || 0, lng: userLng || 0 }}
          zoom={14}
        >
          {riderLat && riderLng && (
            <Marker
              position={{ lat: riderLat, lng: riderLng }}
              icon={riderIcon}
            />
          )}
          {userLat && userLng && (
            <Marker position={{ lat: userLat, lng: userLng }} icon={userIcon} />
          )}
          {restaurantLat && restaurantLng && (
            <Marker
              position={{ lat: userLat, lng: userLng }}
              icon={restaurantIcon}
            />
          )}

          {directionsResponse && (
            <DirectionsRenderer directions={directionsResponse} />
          )}
        </GoogleMap>
        {/* )} */}
      </div>
    </div>
  );
};

export default NotificationUser;

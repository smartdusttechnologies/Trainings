import React from "react";
import { GoogleMap, Marker, DirectionsRenderer } from "@react-google-maps/api";

const RiderMap = ({
  riderLat,
  riderLng,
  userLat,
  userLng,
  restaurantLat,
  restaurantLng,
  directionsResponse,
  directionsError,
  distance,
  duration,
}) => {
  const apiKey = process.env.REACT_APP_GOOGLE_MAPS_API_KEY;

  const riderIcon = {
    url: "/Images/rider.png",
    scaledSize: new window.google.maps.Size(40, 40),
  };

  const restaurantIcon = {
    url: "/Images/restaurant.png",
    scaledSize: new window.google.maps.Size(40, 40),
  };

  const userIcon = {
    url: "/Images/Home.png",
    scaledSize: new window.google.maps.Size(40, 40),
  };

  return (
    <div>
      {riderLat !== null && riderLng !== null ? (
        <div className="alert alert-info" role="alert">
          <strong>Current Location:</strong> Lat: {riderLat}, Lng: {riderLng}
        </div>
      ) : (
        <div className="alert alert-warning" role="alert">
          Fetching your location...
        </div>
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
        center={{ lat: riderLat || 0, lng: riderLng || 0 }}
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

export default RiderMap;

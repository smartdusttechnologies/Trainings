import React from "react";
import RestaurantNotification from "./RestaurantNotification";
import { GoogleMap, Marker, DirectionsRenderer } from "@react-google-maps/api";

const RestaurantMap = (
  riderLat,
  riderLng,
  userLat,
  userLng,
  restaurantLat,
  restaurantLng,
  directionsResponse,
  directionsError,
  distance,
  duration
) => {
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

  return <div></div>;
};

export default RestaurantMap;

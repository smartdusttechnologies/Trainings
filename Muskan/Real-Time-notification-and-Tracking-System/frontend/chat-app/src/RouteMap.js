import React, { useEffect, useState } from "react";
import {
  GoogleMap,
  LoadScript,
  Marker,
  Polyline,
  useJsApiLoader,
} from "@react-google-maps/api";

const RouteMap = () => {
  const [currentLocationIndex, setCurrentLocationIndex] = useState(0);
  const [currentPosition, setCurrentPosition] = useState(null);

  const apiKey = process.env.REACT_APP_GOOGLE_MAPS_API_KEY;

  const { isLoaded, loadError } = useJsApiLoader({
    googleMapsApiKey: apiKey,
  });

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

    // { location: "Danapur", lat: 25.6050, lng: 85.1800 },
    {
      location: "Tasty Kitchen",
      lat: 25.614605699639768,
      lng: 85.04209056402705,
    },
  ];

  // Function to move to the next location on button click
  const moveRider = () => {
    if (currentLocationIndex < route.length - 1) {
      setCurrentLocationIndex((prevIndex) => prevIndex + 1); // Move to the next location
    }
  };

  // Update the rider's position whenever the currentLocationIndex changes
  useEffect(() => {
    setCurrentPosition(route[currentLocationIndex]);
    console.log("Rider moved to: ", route[currentLocationIndex].location);
  }, [currentLocationIndex]);

  if (loadError) return <div>Error loading map</div>;

  return (
    <div>
      <h2>Rider's Route Simulation</h2>
      <button onClick={moveRider}>Move Rider</button>

      {isLoaded && (
        <GoogleMap
          mapContainerStyle={{ height: "500px", width: "100%" }}
          center={route[0]} // Start from Rajendra Nagar
          zoom={12}
        >
          {/* Render route polyline */}
          <Polyline
            path={route}
            options={{
              strokeColor: "#FF0000",
              strokeOpacity: 1,
              strokeWeight: 3,
            }}
          />

          {/* Render rider's current position */}
          {currentPosition && (
            <Marker
              position={currentPosition}
              icon={{
                url: "/images/rider.png",
                scaledSize: new window.google.maps.Size(30, 30),
              }}
            />
          )}
        </GoogleMap>
      )}

      <h3>Current Location: {route[currentLocationIndex]?.location}</h3>
    </div>
  );
};

export default RouteMap;

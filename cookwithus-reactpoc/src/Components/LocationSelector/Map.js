import React, { useState, useEffect } from 'react';
import ReactMapGL, { Marker } from 'react-map-gl';

const Map = ({ latitude, longitude }) => {
  const [viewport, setViewport] = useState({
    latitude: 37.7577,
    longitude: -122.4376,
    zoom: 12,
  });

  useEffect(() => {
    // Update the map viewport when the location changes
    setViewport(prevViewport => ({
      ...prevViewport,
      latitude,
      longitude,
    }));
  }, [latitude, longitude]);

  return (
    <ReactMapGL
      {...viewport}
      width="100%"
      height="400px"
      mapStyle="mapbox://styles/mapbox/streets-v11"
      mapboxApiAccessToken="YOUR_MAPBOX_API_KEY"
      onViewportChange={newViewport => setViewport(newViewport)}
    >
      {/* Marker for the user's location */}
      <Marker latitude={latitude} longitude={longitude} offsetLeft={-20} offsetTop={-10}>
        <div>📍</div>
      </Marker>
    </ReactMapGL>
  );
};

export default Map;

import React, { useState, useEffect } from 'react';
// import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet';
import { MapContainer, Marker, Popup, TileLayer, useMap } from 'react-leaflet'
// import 'leaflet/dist/leaflet.css';
// import 'react-leaflet-markercluster/dist/styles.min.css';

const Map = ({ latitude, longitude, address }) => {
  const [mapCenter, setMapCenter] = useState([latitude, longitude]);

  useEffect(() => {
    setMapCenter([latitude, longitude]);
    console.log([latitude, longitude] , '[latitude, longitude]')
  }, [latitude, longitude]);

  return (
    // <MapContainer center={mapCenter} zoom={13} style={{ height: '100%', width: '100%' }}>
    //   <TileLayer
    //     url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
    //     attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    //   />
    //   <Marker position={mapCenter}>
    //     <Popup>{address}</Popup>
    //   </Marker>
    // </MapContainer>
    <MapContainer center={[51.505, -0.09]} zoom={13} scrollWheelZoom={false}>
        <TileLayer
            attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
            url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
        {/* <Marker position={[51.505, -0.09]} >
            <Popup>{address}</Popup>
        </Marker> */}
    </MapContainer>
  );
};

export default Map;
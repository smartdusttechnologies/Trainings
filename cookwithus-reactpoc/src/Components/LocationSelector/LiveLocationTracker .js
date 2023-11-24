import React, { useState, useEffect } from 'react';
import Map from './Map';
import GoogleMap from './GoogleMap';
import { Box } from '@mui/material';

const LiveLocationTracker = ({}) => {
    const [liveLocation, setLiveLocation] = useState({
      latitude: 0,
      longitude: 0,
    //   address: '',
    });
    
    useEffect(() => {
        navigator.geolocation.getCurrentPosition((position) => {
            setLiveLocation({
              latitude:position.coords.latitude,
              longitude:position.coords.longitude,
            });
            console.log(position.coords.latitude , position.coords.longitude , 'position.coords.')
          });
        const intervalId = setInterval(() => {
          navigator.geolocation.getCurrentPosition((position) => {
            setLiveLocation({
              latitude:position.coords.latitude,
              longitude:position.coords.longitude,
            });
            console.log(position.coords.latitude , position.coords.longitude , 'position.coords.')
          });
        }, 5000); // Update every 5 seconds
    
        // Clean up the interval on component unmount
        return () => clearInterval(intervalId);
      }, []);

  return (
    <Box
        sx={{
        width:'100%',
        height:'43rem',
        }}
    >
      <Box>
        <GoogleMap 
            latitude={liveLocation?.latitude}
            longitude={liveLocation?.longitude}
            zoom={15}
        />
      </Box>
    </Box>
  );
};

export default LiveLocationTracker;

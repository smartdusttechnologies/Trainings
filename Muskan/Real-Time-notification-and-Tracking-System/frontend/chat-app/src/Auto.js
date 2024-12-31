import React, { useState, useRef, useEffect } from 'react';
import { LoadScript, Autocomplete } from '@react-google-maps/api';

const AutocompleteExample = () => {
  const [selectedAddress, setSelectedAddress] = useState('');
  const autocompleteRef = useRef(null);

  const onLoad = (autocomplete) => {
    autocompleteRef.current = autocomplete;
  };

  const handlePlaceChanged = () => {
    const place = autocompleteRef.current.getPlace();
    if (place.geometry) {
      setSelectedAddress(place.formatted_address);
      console.log('Selected Address:', place.formatted_address);
      console.log('Lat:', place.geometry.location.lat());
      console.log('Lng:', place.geometry.location.lng());
    }
  };

  return (
    <LoadScript googleMapsApiKey={process.env.REACT_APP_GOOGLE_MAPS_API_KEY} libraries={['places']}>
      <div>
        <Autocomplete onLoad={onLoad} onPlaceChanged={handlePlaceChanged}>
          <input type="text" placeholder="Search for a place..." className="form-control" />
        </Autocomplete>
        {selectedAddress && (
          <div>
            <h4>Selected Address: {selectedAddress}</h4>
          </div>
        )}
      </div>
    </LoadScript>
  );
};

export default AutocompleteExample;

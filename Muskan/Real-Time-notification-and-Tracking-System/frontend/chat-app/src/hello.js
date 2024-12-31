// import React, { useState, useRef } from 'react';
// import { LoadScript, Autocomplete } from '@react-google-maps/api';
// import GeocodingExample from './Geo';

// const Hello = () => {  
//   const autocompleteRef = useRef(null);
//   const [selectedAddress, setSelectedAddress] = useState('');

//   const autocompleteOnLoad = (autocomplete) => {
//     console.log('Autocomplete Loaded:', autocomplete);
//   };

//   const handlePlaceChanged = () => {
//     const place = autocompleteRef.current.getPlace();
//     if (place.geometry) {
//       setSelectedAddress(place.formatted_address);
//     }
//   };

//   return (
//     <div className="App">
//       <h1>Google Maps Autocomplete and Geocoding</h1>

    
//       <LoadScript googleMapsApiKey={process.env.REACT_APP_GOOGLE_MAPS_API_KEY} libraries={['places']}>
//         <div>
//           <Autocomplete onLoad={autocompleteOnLoad} onPlaceChanged={handlePlaceChanged}>
//             <input type="text" placeholder="Search for a place..." className="form-control" />
//           </Autocomplete>
//           {selectedAddress && (
//             <div>
//               <h4>Selected Address: {selectedAddress}</h4>
//             </div>
//           )}
//         </div>
//       </LoadScript>

     
//       <GeocodingExample />
//     </div>
//   );
// };

// export default Hello;

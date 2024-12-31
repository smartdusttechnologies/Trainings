// import React, { useState } from 'react';
// import { LoadScript } from '@react-google-maps/api';

// const GeocodingExample = () => {
//   const [address, setAddress] = useState('');
//   const [lat, setLat] = useState(null);
//   const [lng, setLng] = useState(null);
//   const [error, setError] = useState(null);

//   const geocodeAddress = async () => {
//     const geocoder = new window.google.maps.Geocoder();
//     try {
//       const response = await geocoder.geocode({ address });
//       if (response.results.length > 0) {
//         const location = response.results[0].geometry.location;
//         setLat(location.lat());
//         setLng(location.lng());
//         console.log('Latitude:', location.lat());
//         console.log('Longitude:', location.lng());
//       } else {
//         setError('No results found for the given address.');
//       }
//     } catch (err) {
//       setError('Error geocoding the address.');
//       console.error(err);
//     }
//   };

//   return (
//     <LoadScript googleMapsApiKey={process.env.REACT_APP_GOOGLE_MAPS_API_KEY}>
//       <div>
//         <input
//           type="text"
//           className="form-control"
//           placeholder="Enter address to geocode"
//           value={address}
//           onChange={(e) => setAddress(e.target.value)}
//         />
//         <button className="btn btn-primary mt-2" onClick={geocodeAddress}>
//           Geocode Address
//         </button>

//         {lat && lng && (
//           <div>
//             <h4>Coordinates:</h4>
//             <p>Latitude: {lat}</p>
//             <p>Longitude: {lng}</p>
//           </div>
//         )}

//         {error && <p className="text-danger">{error}</p>}
//       </div>
//     </LoadScript>
//   );
// };

// export default GeocodingExample;

import React, { useState, useRef, useEffect } from "react";
import axios from "axios";
import {
  Autocomplete,
  useJsApiLoader,
} from "@react-google-maps/api";

const PlaceOrder = () => {
  const [restaurantId, setRestaurantId] = useState("");
  const [userId, setUserId] = useState("");
  const [orderId, setOrderId] = useState("");
  const [userLat, setUserLat] = useState(25.575215031858086);
  const [userLng, setUserLng] = useState(85.04438692975793);
  const [address, setAddress] = useState("");
  const autocompleteRef = useRef(null);
  const [error, setError] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const apiKey = process.env.REACT_APP_GOOGLE_MAPS_API_KEY;

  const { isLoaded } = useJsApiLoader({
    googleMapsApiKey: apiKey,
    libraries: ["places"],
  });

  // const handlePlaceSelected = () => {
  //   const place = autocompleteRef.current.getPlace();
  //   if (place && place.geometry) {
  //     const location = place.geometry.location;
  //     setUserLat(location.lat());
  //     setUserLng(location.lng());
  //     setAddress(place.formatted_address);
  //   } else {
  //     setError("Invalid place selected.");
  //   }
  // };

  const handlePlaceOrder = async () => {
    if (!restaurantId || !userId || !orderId) {
      setError("Please fill in all fields.");
      return;
    }
    setIsLoading(true);
    try {
      const requestBody = {
        restaurantId,
        userId,
        orderId,
        userLocation: {
          latitude: userLat,
          longitude: userLng,
        },
      };

      const response = await axios.post(
        "https://localhost:7076/api/user/place-order",
        requestBody
      );
      alert("Order placed successfully!");
      console.log("Server Response:", response.data);
    } catch (error) {
      console.error("Order placement failed:", error.response?.data || error.message);
      setError("Order placement failed. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="container mt-5">
      <h2 className="text-center mb-4">Place Order</h2>
      <div className="row justify-content-center">
        <div className="col-md-6">
          {error && <div className="alert alert-danger">{error}</div>}
          <div className="form-group mb-3">
            <input
              type="text"
              className="form-control"
              placeholder="Restaurant ID"
              onChange={(e) => setRestaurantId(e.target.value)}
            />
          </div>
          <div className="form-group mb-3">
            <input
              type="text"
              className="form-control"
              placeholder="User ID"
              onChange={(e) => setUserId(e.target.value)}
            />
          </div>
          <div className="form-group mb-3">
            <input
              type="text"
              className="form-control"
              placeholder="Order ID"
              onChange={(e) => setOrderId(e.target.value)}
            />
          </div>
          {/* {isLoaded ? (
            <div className="form-group mb-3">
              <Autocomplete
                onLoad={(autocomplete) => (autocompleteRef.current = autocomplete)}
                onPlaceChanged={handlePlaceSelected}
              >
                <input
                  type="text"
                  className="form-control"
                  placeholder="Search Address"
                />
              </Autocomplete>
            </div>
          ) : (
            <p>Loading Google Maps...</p>
          )} */}
          <div className="d-grid">
            <button
              className="btn btn-primary"
              onClick={handlePlaceOrder}
              disabled={isLoading}
            >
              {isLoading ? "Placing Order..." : "Place Order"}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default PlaceOrder;


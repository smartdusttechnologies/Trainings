import React from "react";
import "./App.css";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import PlaceOrder from "./Components/User/PlaceOrder";
import NotificationUser from "./Components/User/NotificationUser";
import RestaurantNotification from "./Components/Restaurant/RestaurantNotification";
import RiderNotification from "./Components/Rider/RiderNotification";
// import LocationTracker from "./RouteMap";
// import Hello from "./hello";
// import Rider2Notification from "./Components/Rider/Rider2Notification";
import Location from "./Location";
import RouteMap from "./RouteMap";
// import Rider2Notification from "./Components/Rider/Rider2Notification";
// import Hello from "./Components/Rider/hello";
// import RiderLocationComponent from "./Components/Rider/RiderLocationComponent";

function App() {
  const userId = "456";
  return (
    <Router>
      <Routes>
        <Route
          path="/user/place-order"
          element={<PlaceOrder userId={userId} />}
        />
        <Route path="/" element={<Location />} />
        {/* <Route path="/1" element={<Hello />} /> */}
        <Route path="/route" element={<RouteMap />} />
        <Route
          path="/user/notifications"
          element={<NotificationUser userId={userId} />}
        />
        {/* <Route path="/rider" element={<Hello />} /> */}
        <Route
          path="/restaurant/notifications"
          element={<RestaurantNotification />}
        />
        <Route path="/rider/notifications" element={<RiderNotification />} />
        {/* <Route path="/rider/notifications" element={<Rider2Notification />} /> */}
      </Routes>
    </Router>
  );
}

export default App;

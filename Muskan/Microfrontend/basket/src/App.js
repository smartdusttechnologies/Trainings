import React from "react";
import {
  Routes,
  Route,
  unstable_HistoryRouter as HistoryRouter,
} from "react-router-dom";
import Home from "./Pages/Home";
import BasketList from "./Component/BasketList";
// import ProductList from "./Component/ProductList";
// import ProductDetails from "./Component/ProductDetails";
import { AuthProvider } from "auth/AuthProvider";
// import Checkout from "./Component/Checkout";
import Checkout from "./Component/Checkout";
import AddToCart from "./Component/AddToCart";
const App = ({ history }) => {
  return (
    <AuthProvider>
      <HistoryRouter history={history}>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/basket/list" element={<BasketList />} />
          <Route path="/basket/cart" element={<AddToCart />} />
          <Route path="/basket/checkout" element={<Checkout />} />
          {/* <Route path="/product/:id" element={<ProductDetails />} /> */}
        </Routes>
      </HistoryRouter>
    </AuthProvider>
  );
};

export default App;

import React from "react";
import {
  Routes,
  Route,
  unstable_HistoryRouter as HistoryRouter,
} from "react-router-dom";
import ProductList from "./Component/ProductList";
import ProductDetails from "./Component/ProductDetails";
import Home from "./Pages/Hoem";
import { AuthProvider } from "auth/AuthProvider";
const App = ({ history }) => {
  return (
    <AuthProvider>
      <HistoryRouter history={history}>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/product/list" element={<ProductList />} />
          <Route path="/product/:id" element={<ProductDetails />} />
        </Routes>
      </HistoryRouter>
    </AuthProvider>
  );
};

export default App;

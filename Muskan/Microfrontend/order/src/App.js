import React from "react";
import {
  Routes,
  Route,
  unstable_HistoryRouter as HistoryRouter,
} from "react-router-dom";

import Home from "./Pages/Hoem";
import { AuthProvider } from "auth/AuthProvider";
import OrderList from "./Component/Orderlist";
const App = ({ history }) => {
  return (
    <AuthProvider>
      <HistoryRouter history={history}>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/order/list" element={<OrderList />} />
        </Routes>
      </HistoryRouter>
    </AuthProvider>
  );
};

export default App;

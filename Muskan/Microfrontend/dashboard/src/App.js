import React from "react";
import {
  Routes,
  Route,
  unstable_HistoryRouter as HistoryRouter,
} from "react-router-dom";

import { AuthProvider } from "auth/AuthProvider";
import ProtectedRoute from "./Component/ProtectedRoute";
import Home from "./Pages/Home";
import Unauthorized from "./Component/Unauthorized";
import Dashboard from "./Pages/Dashboard";
import Callback from "auth/Callback";
const App = ({ history }) => {
  return (
    <>
      <AuthProvider>
        <HistoryRouter history={history}>
          <Routes>
            <Route
              path="/dashboard"
              element={
                <ProtectedRoute>
                  <Dashboard />
                </ProtectedRoute>
              }
            />

            <Route path="/*" element={<Home />} />
            <Route path="/callback" element={<Callback />} />
            <Route path="/auth/login" element={<Unauthorized />} />
          </Routes>
        </HistoryRouter>
      </AuthProvider>
    </>
  );
};

export default App;

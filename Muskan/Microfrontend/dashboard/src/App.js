import React from "react";
import {
  Routes,
  Route,
  unstable_HistoryRouter as HistoryRouter,
} from "react-router-dom";
import Dashboard from "./Component/Dashboard";
const App = ({ history }) => {
  return (
    <>
      <HistoryRouter history={history}>
        <Routes>
          <Route path="/dashboard" element={<Dashboard />} />
        </Routes>
      </HistoryRouter>
    </>
  );
};

export default App;

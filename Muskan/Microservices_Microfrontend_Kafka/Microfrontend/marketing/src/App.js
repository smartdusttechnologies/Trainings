import React from "react";
import {
  BrowserRouter,
  Routes,
  Route,
  unstable_HistoryRouter as HistoryRouter,
} from "react-router-dom";
// import { StyledEngineProvider } from '@mui/material/styles';
import LandingPage from "./Pages/Landing";
// import Header from "./Component/Header";
import Pricing from "./Pages/Pricing";
// import Pricing from "./Pages/Pricing";
const App = ({ history }) => {
  // console.log("BrowserRouter :", BrowserRouter);
  // console.log("Routes :", Routes);
  // console.log("Route :", Route);
  // console.log("LandingPage :", LandingPage);
  // console.log("HistoryRouter :", HistoryRouter);
  // console.log("hist :", Pricing);
  // console.log("Pricing : ", Pricing);
  return (
    <>
      <HistoryRouter history={history}>
        {/* <Header /> */}
        <Routes>
          <Route path="/" element={<LandingPage />} />
          <Route path="/pricing" element={<Pricing />} />
        </Routes>
      </HistoryRouter>
    </>
  );
};

export default App;

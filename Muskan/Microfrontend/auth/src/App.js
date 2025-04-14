import React from "react";
import {
  BrowserRouter,
  Routes,
  Route,
  unstable_HistoryRouter as HistoryRouter,
} from "react-router-dom";
import Login from "./Component/Login";
import Signup from "./Component/SignIn";
import AuthHeader from "./Component/AuthHeader";
// import Header from
const App = ({ history, onSignIn }) => {
  return (
    <>
      <HistoryRouter history={history}>
        <AuthHeader />
        <Routes>
          <Route path="/auth/signup" element={<Signup onSignIn={onSignIn} />} />
          <Route path="/auth/login" element={<Login onSignIn={onSignIn} />} />
        </Routes>
      </HistoryRouter>
    </>
  );
};

export default App;

import React, { useEffect } from "react";
import {
  BrowserRouter,
  Routes,
  Route,
  unstable_HistoryRouter as HistoryRouter,
} from "react-router-dom";
// import Login from "./Component/Login";
// import Signup from "./Component/SignIn";
import AuthHeader from "./Component/AuthHeader";
import { AuthProvider } from "./Context/AuthProvider";
import Profile from "./Component/Profile";
import Callback from "./Component/Callback";
import LoginButton from "./Component/Login";
import LogoutButton from "./Component/Logout";
import { useAuth0 } from "@auth0/auth0-react";
import Unauthorized from "./Pages/LoginPage";
const App = ({ history, onSignIn }) => {
  const { user, isAuthenticated, isLoading } = useAuth0();
  useEffect(() => {
    console.log("isAuthenticated:", isAuthenticated);
    console.log("user:", user);
    if (isAuthenticated) {
      console.log("User authenticated:", user);
      onSignIn(); // Call onSignIn when user is authenticated
    }
  }, [isAuthenticated, onSignIn, user]); // Ensure user is a dependency

  return (
    <AuthProvider>
      <HistoryRouter history={history}>
        <AuthHeader onSignIn={onSignIn} />
        <Routes>
          <Route path="/auth/callback" element={<Callback />} />

          <Route path="/auth/login" element={<Unauthorized />} />
          <Route path="/auth/logout" element={<LogoutButton />} />
          <Route path="/auth/profile" element={<Profile />} />
        </Routes>
      </HistoryRouter>
    </AuthProvider>
  );
};

export default App;

import React, { lazy, Suspense, useState, useEffect } from "react";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom"; // Use Navigate instead of Redirect in v6
import Progress from "./Component/Progress";
import Header from "./Component/Header";

const MarketLazy = lazy(() => import("./Component/MarketingApp"));
const AuthLazy = lazy(() => import("./Component/AuthApp"));
const DashboardLazy = lazy(() => import("./Component/Dashboard"));

const App = () => {
  // State to track if the user is signed in
  const [isSignIn, setIsSignIn] = useState(false);
  // Effect to log when the user signs in
  useEffect(() => {
    if (isSignIn) {
      console.log("User is signed in");
    }
  }, [isSignIn]); // Runs whenever `isSignIn` changes

  return (
    <BrowserRouter>
      <Header OnSignOut={() => setIsSignIn(false)} isSignIn={isSignIn} />
      <Suspense fallback={<Progress />}>
        <Routes>
          <Route
            path="/auth/signup"
            element={<AuthLazy onSignIn={() => setIsSignIn(true)} />}
          />
          <Route
            path="/auth/login"
            element={<AuthLazy onSignIn={() => setIsSignIn(true)} />}
          />
          <Route path="/" element={<MarketLazy />} />
          <Route
            path="/dashboard"
            element={
              isSignIn ? (
                <DashboardLazy />
              ) : (
                <Navigate to="/auth/login" replace />
              )
            }
          />
          <Route path="/pricing" element={<MarketLazy />} />
        </Routes>
      </Suspense>
    </BrowserRouter>
  );
};

export default App;

import React, { lazy, Suspense, useState } from "react";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Progress from "./Component/Progress";
import Header from "./Component/Header";
import ErrorBoundary from "./Component/ErrorBooundary";
import { useAuth } from "auth/useAuth";
import { AuthProvider } from "auth/AuthProvider";
// import Callback from "./Component/Callback";
import Callback from "auth/Callback";
const MarketLazy = lazy(() => import("./Component/MarketingApp"));
const AuthLazy = lazy(() => import("./Component/AuthApp"));
const DashboardLazy = lazy(() => import("./Component/Dashboard"));

const ProtectedRoute = ({ children }) => {
  const { isAuthenticated, isLoading } = useAuth();
  console.log("is Authenticated from the protected routes : ", isAuthenticated);
  if (isLoading) return <Progress />;

  return isAuthenticated ? children : <Navigate to="/auth/login" replace />;
};

const App = () => {
  const [isSignIn, setIsSignIn] = useState(false);
  const { isAuthenticated, isLoading } = useAuth();
  console.log("isAuthenticated from the home", isAuthenticated);
  return (
    <AuthProvider>
      <ErrorBoundary>
        <BrowserRouter>
          <Header OnSignOut={() => setIsSignIn(false)} isSignIn={isSignIn} />
          <Suspense fallback={<Progress />}>
            <Routes>
              <Route
                path="/auth/*"
                element={<AuthLazy onSignIn={() => setIsSignIn(true)} />}
              />
              <Route path="/callback" element={<Callback />} />
              <Route path="/*" element={<MarketLazy />} />
              <Route
                path="/dashboard"
                element={
                  <ProtectedRoute>
                    <DashboardLazy />
                  </ProtectedRoute>
                }
              />
            </Routes>
          </Suspense>
        </BrowserRouter>
      </ErrorBoundary>
    </AuthProvider>
  );
};

export default App;

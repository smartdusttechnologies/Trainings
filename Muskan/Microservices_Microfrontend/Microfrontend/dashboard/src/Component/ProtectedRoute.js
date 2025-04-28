import React from "react";
import { Navigate } from "react-router-dom";
import { LinearProgress } from "@mui/material";
import { useAuth } from "auth/useAuth";

const ProtectedRoute = ({ children }) => {
  const { isAuthenticated, isLoading } = useAuth();

  if (isLoading) return <LinearProgress />;

  if (!isAuthenticated) {
    return <Navigate to="/auth/login" replace />;
  }

  return children;
};

export default ProtectedRoute;

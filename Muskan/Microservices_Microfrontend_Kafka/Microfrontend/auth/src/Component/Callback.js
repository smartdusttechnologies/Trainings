import React, { useEffect } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { LinearProgress } from "@mui/material";
import { useNavigate } from "react-router-dom";

const Callback = () => {
  const { loginWithRedirect, handleRedirectCallback } = useAuth0();
  const navigate = useNavigate();

  useEffect(() => {
    const handleLoginRedirect = async () => {
      try {
        await handleRedirectCallback();
        navigate("/auth/profile"); // Redirect to profile or another page after successful login
      } catch (error) {
        console.error("Login error", error);
      }
    };

    handleLoginRedirect();
  }, [handleRedirectCallback, navigate]);

  return (
    <div>
      <LinearProgress />
    </div>
  );
};

export default Callback;

import React, { useEffect } from "react";
import { useAuth0 } from "@auth0/auth0-react";
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

  return <div>Loading...</div>; // You can show a loading spinner or message while processing
};

export default Callback;

import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { Button } from "@mui/material";

const LoginButton = ({ onSignIn }) => {
  const { loginWithRedirect } = useAuth0();

  const handleLogin = async () => {
    await loginWithRedirect();
    onSignIn();
    console.log(onSignIn());
  };

  return (
    <Button
      onClick={handleLogin}
      sx={{
        color: "black",
        backgroundColor: "white",
        border: "1px solid #003366",
        boxShadow: "0px 4px 10px rgba(0, 0, 0, 0.3)",
        "&:hover": {
          backgroundColor: "rgba(255, 255, 255, 0.1)",
        },
      }}
    >
      Log In
    </Button>
  );
};

export default LoginButton;

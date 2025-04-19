import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { Button } from "@mui/material";

const LogoutButton = () => {
  const { logout } = useAuth0();

  // const handleLogout = () => {
  //   logout({ returnTo: window.location.origin });
  // };

  return (
    <Button
      sx={{
        color: "black",
        backgroundColor: "white",
        border: "none",
        boxShadow: "none",
        "&:hover": {
          backgroundColor: "rgba(255, 255, 255, 0.1)",
        },
      }}
      onClick={() =>
        logout({
          logoutParams: {
            returnTo: window.location.origin,
          },
        })
      }
    >
      Log Out
    </Button>
  );
};

export default LogoutButton;

import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { Button, Link, Typography } from "@mui/material";
import LoginButton from "./Login"; // Assuming you have these components
import LogoutButton from "./Logout"; // Assuming you have these components

const Main = () => {
  const { user, isAuthenticated, isLoading } = useAuth0();

  if (isLoading) {
    return <Typography>Loading...</Typography>; // Show loading text while checking authentication
  }

  return (
    <div>
      {!isAuthenticated && (
        <Button variant="contained" color="primary" onClick={LoginButton}>
          Login
        </Button>
      )}
      {isAuthenticated && (
        <>
          <Button variant="contained" color="secondary" onClick={LogoutButton}>
            Logout
          </Button>
          <Link
            href="/profile"
            color="inherit"
            sx={{
              display: "block",
              marginTop: 2,
              textDecoration: "none",
            }}
          >
            Profile
          </Link>
        </>
      )}
    </div>
  );
};

export default Main;

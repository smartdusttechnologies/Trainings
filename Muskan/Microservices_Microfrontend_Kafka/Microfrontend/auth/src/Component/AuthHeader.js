import React, { useState } from "react";
import { NavLink } from "react-router-dom";
import { Divider, Box, Typography } from "@mui/material";
import { useAuth0 } from "@auth0/auth0-react";
import LoginButton from "./Login";
import LogoutButton from "./Logout";
const AuthHeader = ({ onSignIn }) => {
  const [isSignedIn, setIsSignedIn] = useState(false);

  const { user, isAuthenticated, isLoading } = useAuth0();
  console.log(onSignIn);
  if (isLoading) {
    return <Typography>Loading...</Typography>;
  }
  const handleSignIn = () => {
    setIsSignedIn(true); // Set to true when the user logs in
    console.log("User signed in");
  };
  return (
    <>
      <Box display="flex" justifyContent="center" gap={4} sx={{ mb: 2 }}>
        {!isAuthenticated && <LoginButton onSignIn={handleSignIn} />}
        {isAuthenticated && <LogoutButton />}
        {isAuthenticated && (
          <NavLink
            to="/auth/profile"
            style={({ isActive }) => ({
              textDecoration: "none",
              color: isActive ? "#1976d2" : "#555",
              fontWeight: isActive ? "bold" : "normal",
              borderBottom: isActive ? "2px solid #1976d2" : "none",
              paddingBottom: "4px",
            })}
          >
            Profile
          </NavLink>
        )}
      </Box>
      <Divider sx={{ mb: 2 }} />
    </>
  );
};

export default AuthHeader;

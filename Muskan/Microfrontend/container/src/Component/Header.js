import React from "react";
import { AppBar, Toolbar, Typography, Box } from "@mui/material";
import { NavLink } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import LoginButton from "auth/LoginButton";
import LogoutButton from "auth/LogoutButton";

const Header = ({ onSignOut, isSignIn }) => {
  const { isAuthenticated } = useAuth0();

  return (
    <AppBar position="static" sx={{ backgroundColor: "#003366" }}>
      <Toolbar>
        <Typography variant="h6" sx={{ flexGrow: 1, color: "white" }}>
          iApp
        </Typography>

        <NavLink
          to="/"
          style={{ marginLeft: "16px", textDecoration: "none", color: "white" }}
        >
          Home
        </NavLink>

        <NavLink
          to="/pricing"
          style={{ marginLeft: "16px", textDecoration: "none", color: "white" }}
        >
          Pricing
        </NavLink>

        {isAuthenticated && (
          <>
            {" "}
            <NavLink
              to="/dashboard"
              style={{
                marginLeft: "16px",
                textDecoration: "none",
                color: "white",
              }}
            >
              Dashboard
            </NavLink>
            <NavLink
              to="/auth/profile"
              style={{
                marginLeft: "16px",
                textDecoration: "none",
                color: "white",
              }}
            >
              Profile
            </NavLink>
          </>
        )}

        <Box sx={{ marginLeft: "auto" }}>
          {isAuthenticated ? (
            <LogoutButton />
          ) : (
            <LoginButton onSignIn={() => console.log("User signed in")} />
          )}
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Header;

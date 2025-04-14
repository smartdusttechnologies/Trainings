import React from "react";
import { AppBar, Toolbar, Typography, Button } from "@mui/material";
import { NavLink } from "react-router-dom";

const Header = ({ OnSignOut, isSignIn }) => {
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

        {isSignIn ? (
          <>
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
            <Button
              color="inherit"
              sx={{ marginLeft: "16px" }}
              onClick={OnSignOut}
            >
              Sign Out
            </Button>
          </>
        ) : (
          <>
            <NavLink
              to="/auth/login"
              style={{
                marginLeft: "16px",
                textDecoration: "none",
                color: "white",
              }}
            >
              Login
            </NavLink>

            <NavLink
              to="/auth/signup"
              style={{
                marginLeft: "16px",
                textDecoration: "none",
                color: "white",
              }}
            >
              Sign Up
            </NavLink>
          </>
        )}
      </Toolbar>
    </AppBar>
  );
};

export default Header;

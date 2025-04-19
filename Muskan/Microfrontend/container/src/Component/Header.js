import React from "react";
import { AppBar, Toolbar, Typography, Box } from "@mui/material";
import { NavLink } from "react-router-dom";
import { useAuth } from "auth/useAuth";
import LoginButton from "auth/LoginButton";
import LogoutButton from "auth/LogoutButton";

const Header = ({ onSignOut, isSignIn }) => {
  const { isAuthenticated } = useAuth();

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
        <NavLink
          to="/product/list"
          style={{ marginLeft: "16px", textDecoration: "none", color: "white" }}
        >
          Product
        </NavLink>
        <NavLink
          to="/order/list"
          style={{ marginLeft: "16px", textDecoration: "none", color: "white" }}
        >
          Order
        </NavLink>
        <NavLink
          to="/basket/list"
          style={{ marginLeft: "16px", textDecoration: "none", color: "white" }}
        >
          Basket
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

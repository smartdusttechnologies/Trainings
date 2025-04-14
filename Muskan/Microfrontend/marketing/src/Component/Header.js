import React from "react";
import { AppBar, Toolbar, Typography, Button } from "@mui/material";
import { NavLink } from "react-router-dom";

const Header = () => {
  return (
    <div>
      {/* Header */}
      <AppBar position="static" sx={{ backgroundColor: "#003366" }}>
        <Toolbar>
          <Typography variant="h6" sx={{ flexGrow: 1, color: "white" }}>
            iApp
          </Typography>
          <NavLink
            to="/pricing"
            style={{
              marginLeft: "16px",
              textDecoration: "none", // Remove underline
              color: "white", // White text for links
            }}
            activestyle={{
              fontWeight: "bold",
              color: "#ffcc00", // Highlight active link with yellow
            }}
          >
            Pricing
          </NavLink>
          <NavLink
            to="/"
            style={{
              marginLeft: "16px",
              textDecoration: "none", // Remove underline
              color: "white", // White text for links
            }}
            activestyle={{
              fontWeight: "bold",
              color: "#ffcc00", // Highlight active link with yellow
            }}
          >
            Home
          </NavLink>
        </Toolbar>
      </AppBar>
    </div>
  );
};

export default Header;

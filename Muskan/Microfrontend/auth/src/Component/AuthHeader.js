import React from "react";
import { NavLink } from "react-router-dom";
import { Divider, Box } from "@mui/material";

const AuthHeader = () => {
  return (
    <>
      <Box display="flex" justifyContent="center" gap={4} sx={{ mb: 2 }}>
        <NavLink
          to="/auth/login"
          style={({ isActive }) => ({
            textDecoration: "none",
            color: isActive ? "#1976d2" : "#555",
            fontWeight: isActive ? "bold" : "normal",
            borderBottom: isActive ? "2px solid #1976d2" : "none",
            paddingBottom: "4px",
          })}
        >
          Login
        </NavLink>
        <NavLink
          to="/auth/signup"
          style={({ isActive }) => ({
            textDecoration: "none",
            color: isActive ? "#1976d2" : "#555",
            fontWeight: isActive ? "bold" : "normal",
            borderBottom: isActive ? "2px solid #1976d2" : "none",
            paddingBottom: "4px",
          })}
        >
          SignUp
        </NavLink>
      </Box>
      <Divider sx={{ mb: 2 }} />
    </>
  );
};

export default AuthHeader;

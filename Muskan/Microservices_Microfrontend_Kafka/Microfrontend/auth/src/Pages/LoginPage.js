// Unauthorized.js
import React from "react";
import { Box, Typography, Button } from "@mui/material";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import { useNavigate } from "react-router-dom";
import LoginButton from "../Component/Login";

const Unauthorized = () => {
  const navigate = useNavigate();

  return (
    <Box
      sx={{
        height: "100vh",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        textAlign: "center",
        bgcolor: "#f5f5f5",
        padding: 2,
      }}
    >
      <LockOutlinedIcon sx={{ fontSize: 60, color: "#d32f2f", mb: 2 }} />
      <Typography variant="h4" gutterBottom>
        You are not authorized
      </Typography>
      <Typography variant="body1" sx={{ mb: 3 }}>
        Please log in to access this page.
      </Typography>
      <LoginButton
        sx={{
          color: "black",
          backgroundColor: "#ffffff",
          border: "1px solid #003366",
          boxShadow: "0px 4px 10px rgba(0, 0, 0, 0.2)",
          "&:hover": {
            backgroundColor: "#f0f0f0",
            boxShadow: "0px 6px 14px rgba(0, 0, 0, 0.3)",
          },
        }}
      />
    </Box>
  );
};
export default Unauthorized;

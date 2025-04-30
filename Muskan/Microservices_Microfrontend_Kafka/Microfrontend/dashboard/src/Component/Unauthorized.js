// Unauthorized.js
import React from "react";
import { Box, Typography, Button } from "@mui/material";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import { useNavigate } from "react-router-dom";

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
    </Box>
  );
};
export default Unauthorized;

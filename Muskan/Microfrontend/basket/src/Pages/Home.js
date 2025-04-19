import { AppBar, Typography, Box, Container } from "@mui/material";
import React from "react";
import { NavLink } from "react-router-dom";
import Navbar from "../Component/Navbar";
const Home = () => {
  return (
    <div>
      <AppBar
        position="fixed"
        sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}
      >
        <Navbar />
      </AppBar>
      <Box sx={{ mt: 10 }}>
        <Container>
          <Typography variant="h4" gutterBottom>
            Welcome to the Basket Home
          </Typography>

          <Typography variant="body1">
            This is the home page of your basket.
          </Typography>
        </Container>
      </Box>
    </div>
  );
};

export default Home;

import React from "react";
import {
  AppBar,
  Toolbar,
  Typography,
  Button,
  Box,
  Container,
  Grid,
  Paper,
} from "@mui/material";
import { Star, AccessTime, Security } from "@mui/icons-material";
import { NavLink } from "react-router-dom";

const features = [
  {
    icon: <Star fontSize="large" />,
    title: "Top Quality",
    description: "We deliver only the best to our customers.",
  },
  {
    icon: <AccessTime fontSize="large" />,
    title: "24/7 Support",
    description: "Get help anytime with our round-the-clock support.",
  },
  {
    icon: <Security fontSize="large" />,
    title: "Secure",
    description: "We ensure your data and privacy is protected.",
  },
];

const LandingPage = () => {
  return (
    <div>
      {/* Hero Section */}
      <Box
        sx={{
          backgroundColor: "#f5f5f5",
          py: 8,
          textAlign: "center",
        }}
      >
        <Container maxWidth="md">
          <Typography variant="h3" gutterBottom>
            Welcome to MyBrand
          </Typography>
          <Typography variant="h6" paragraph>
            Build something amazing with our platform. It's fast, secure, and
            easy to use.
          </Typography>
          <Button variant="contained" color="primary" size="large">
            Get Started
          </Button>
          <NavLink
            to="/pricing"
            style={{
              marginLeft: "16px",
              textDecoration: "none",
              color: "black",
            }}
            activestyle={{
              fontWeight: "bold",
              color: "#ffcc00",
            }}
          >
            Price
          </NavLink>
        </Container>
      </Box>

      {/* Features Section */}
      <Box sx={{ py: 6 }}>
        <Container maxWidth="lg">
          <Grid container spacing={4}>
            {features.map((feature, index) => (
              <Grid item xs={12} md={4} key={index}>
                <Paper elevation={3} sx={{ p: 4, textAlign: "center" }}>
                  {feature.icon}
                  <Typography variant="h6" sx={{ mt: 2 }}>
                    {feature.title}
                  </Typography>
                  <Typography variant="body1" sx={{ mt: 1 }}>
                    {feature.description}
                  </Typography>
                </Paper>
              </Grid>
            ))}
          </Grid>
        </Container>
      </Box>

      {/* Footer */}
      <Box
        sx={{
          backgroundColor: "#1976d2",
          color: "#fff",
          py: 3,
          textAlign: "center",
        }}
      >
        <Typography variant="body1">
          © 2025 MyBrand. All rights reserved.
        </Typography>
      </Box>
    </div>
  );
};
export default LandingPage;

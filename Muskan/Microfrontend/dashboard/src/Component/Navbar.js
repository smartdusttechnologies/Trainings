import React from "react";
import { AppBar, Toolbar, Typography, Box, Button } from "@mui/material";
import { Link } from "react-router-dom";
import LoginButton from "auth/LoginButton";
import LogoutButton from "auth/LogoutButton";
import { useAuth } from "auth/useAuth";

const Navbar = () => {
  const { isAuthenticated } = useAuth();
  return (
    <AppBar position="static" color="primary">
      <Toolbar>
        <Typography variant="h6" sx={{ flexGrow: 1 }}>
          MyApp
        </Typography>

        <Box sx={{ display: "flex", gap: 2 }}>
          <Button color="inherit" component={Link} to="/">
            Home
          </Button>
          {/* <Button color="inherit" component={Link} to="/profile">
            Profile
          </Button> */}
          <Box sx={{ marginLeft: "auto" }}>
            {isAuthenticated ? (
              <>
                {" "}
                <LogoutButton />
                <Button color="inherit" component={Link} to="/dashboard">
                  Dashboard
                </Button>
              </>
            ) : (
              <LoginButton onSignIn={() => console.log("User signed in")} />
            )}
          </Box>
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Navbar;

import React, { useEffect } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import {
  Card,
  CardContent,
  Typography,
  Avatar,
  CircularProgress,
  Box,
} from "@mui/material";
import EmailIcon from "@mui/icons-material/Email";
import PersonIcon from "@mui/icons-material/Person";

const Profile = () => {
  const { user, isAuthenticated, isLoading, getAccessTokenSilently } =
    useAuth0();
  console.log(user);
  useEffect(() => {
    const getToken = async () => {
      try {
        const token = await getAccessTokenSilently();
        console.log("Access Token:", token);
      } catch (e) {
        console.error("Error getting access token:", e);
      }
    };

    getToken();
  }, [getAccessTokenSilently]);

  if (isLoading) {
    return (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        height="100vh"
      >
        <CircularProgress />
      </Box>
    );
  }

  return (
    isAuthenticated && (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        height="100vh"
        bgcolor="background.default"
        padding={2}
      >
        <Card sx={{ maxWidth: 400, width: "100%" }} elevation={4}>
          <CardContent
            sx={{
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
              textAlign: "center",
            }}
          >
            <Avatar
              src={user?.picture}
              alt={user?.name}
              sx={{ width: 100, height: 100, marginBottom: 2 }}
            />
            <Typography
              variant="h5"
              component="div"
              color="primary.main"
              sx={{ fontWeight: "bold", display: "flex", alignItems: "center" }}
            >
              <PersonIcon sx={{ marginRight: 1 }} /> {user?.name}
            </Typography>
            <Typography
              variant="body1"
              color="textSecondary"
              sx={{ display: "flex", alignItems: "center", marginTop: 1 }}
            >
              <EmailIcon sx={{ marginRight: 1 }} /> {user?.email}
            </Typography>
          </CardContent>
        </Card>
      </Box>
    )
  );
};

export default Profile;

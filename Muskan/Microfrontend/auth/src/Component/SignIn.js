import React, { useState } from "react";
import { TextField, Button, Box } from "@mui/material";
import AuthHeader from "./AuthHeader";

const Signup = ({ onSignIn }) => {
  const [formData, setFormData] = useState({
    name: "",
    email: "",
    password: "",
  });

  const handleChange = (e) =>
    setFormData({ ...formData, [e.target.name]: e.target.value });

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log("Signup data:", formData);
    alert("Signup successful!");
  };

  return (
    <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 2 }}>
      {/* <AuthHeader /> */}
      <TextField
        fullWidth
        label="Name"
        name="name"
        value={formData.name}
        onChange={handleChange}
        margin="normal"
        required
      />
      <TextField
        fullWidth
        label="Email"
        name="email"
        type="email"
        value={formData.email}
        onChange={handleChange}
        margin="normal"
        required
      />
      <TextField
        fullWidth
        label="Password"
        name="password"
        type="password"
        value={formData.password}
        onChange={handleChange}
        margin="normal"
        required
      />
      <Button
        onClick={onSignIn}
        type="submit"
        fullWidth
        variant="contained"
        sx={{ mt: 2 }}
      >
        Signup
      </Button>
    </Box>
  );
};

export default Signup;

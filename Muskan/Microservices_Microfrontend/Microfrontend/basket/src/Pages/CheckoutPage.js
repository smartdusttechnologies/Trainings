import React, { useState } from "react";
import {
  Container,
  Grid,
  Typography,
  TextField,
  MenuItem,
  Button,
  List,
  ListItem,
  ListItemText,
  Card,
  CardContent,
  InputAdornment,
  Breadcrumbs,
  Link,
} from "@mui/material";
import { ShoppingCart, LocalOffer } from "@mui/icons-material";

const states = [
  "Andhra Pradesh",
  "Arunachal Pradesh",
  "Assam",
  "Bihar",
  "Chhattisgarh",
  "Goa",
  "Gujarat",
  "Haryana",
  "Himachal Pradesh",
  "Jharkhand",
  "Karnataka",
  "Kerala",
  "Madhya Pradesh",
  "Maharashtra",
  "Manipur",
  "Meghalaya",
  "Mizoram",
  "Nagaland",
  "Odisha",
  "Punjab",
  "Rajasthan",
  "Sikkim",
  "Tamil Nadu",
  "Telangana",
  "Tripura",
  "Uttar Pradesh",
  "Uttarakhand",
  "West Bengal",
  "Andaman and Nicobar Islands",
  "Chandigarh",
  "Dadra and Nagar Haveli and Daman and Diu",
  "Delhi",
  "Jammu and Kashmir",
  "Ladakh",
  "Lakshadweep",
  "Puducherry",
];

const countryList = ["India"];

export default function CheckoutPage({ cart }) {
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    username: "",
    emailAddress: "",
    addressLine: "",
    address2: "",
    country: "",
    state: "",
    zipCode: "",
  });

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  return (
    <Container>
      <Breadcrumbs aria-label="breadcrumb" sx={{ mt: 2 }}>
        <Link underline="hover" color="inherit" href="/">
          Home
        </Link>
        <Link underline="hover" color="inherit" href="/cart">
          Cart
        </Link>
        <Typography color="text.primary">Checkout</Typography>
      </Breadcrumbs>

      <Grid container spacing={4} sx={{ mt: 2 }}>
        <Grid item xs={12} md={4}>
          <Typography variant="h6">Your Cart</Typography>
          <List>
            {cart.items.map((item, index) => (
              <ListItem key={index} divider>
                <ListItemText
                  primary={item.productName}
                  secondary={`Quantity: ${item.quantity}`}
                />
                <Typography variant="body2">${item.price}</Typography>
              </ListItem>
            ))}
            <ListItem>
              <ListItemText primary="Total (USD)" />
              <Typography variant="subtitle1">
                <strong>${cart.totalPrice}</strong>
              </Typography>
            </ListItem>
          </List>

          <Card>
            <CardContent>
              <TextField
                fullWidth
                placeholder="Promo code"
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <LocalOffer />
                    </InputAdornment>
                  ),
                }}
              />
              <Button variant="contained" fullWidth sx={{ mt: 2 }}>
                Redeem
              </Button>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} md={8}>
          <Typography variant="h6" gutterBottom>
            Billing Address
          </Typography>
          <Grid container spacing={2}>
            <Grid item xs={12} sm={6}>
              <TextField
                name="firstName"
                label="First name"
                fullWidth
                value={formData.firstName}
                onChange={handleChange}
                required
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                name="lastName"
                label="Last name"
                fullWidth
                value={formData.lastName}
                onChange={handleChange}
                required
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                name="username"
                label="Username"
                fullWidth
                value={formData.username}
                onChange={handleChange}
                required
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">@</InputAdornment>
                  ),
                }}
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                name="emailAddress"
                label="Email (optional)"
                fullWidth
                value={formData.emailAddress}
                onChange={handleChange}
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                name="addressLine"
                label="Address"
                fullWidth
                value={formData.addressLine}
                onChange={handleChange}
                required
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                name="address2"
                label="Address 2 (optional)"
                fullWidth
                value={formData.address2}
                onChange={handleChange}
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                name="country"
                label="Country"
                fullWidth
                select
                value={formData.country}
                onChange={handleChange}
                required
              >
                {countryList.map((country) => (
                  <MenuItem key={country} value={country}>
                    {country}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>
            <Grid item xs={12} sm={4}>
              <TextField
                name="state"
                label="State"
                fullWidth
                select
                value={formData.state}
                onChange={handleChange}
                required
              >
                {states.map((state) => (
                  <MenuItem key={state} value={state}>
                    {state}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>
            <Grid item xs={12} sm={2}>
              <TextField
                name="zipCode"
                label="Zip"
                fullWidth
                value={formData.zipCode}
                onChange={handleChange}
                required
              />
            </Grid>
          </Grid>

          <Button variant="contained" color="primary" sx={{ mt: 3 }}>
            Proceed to Payment
          </Button>
        </Grid>
      </Grid>
    </Container>
  );
}

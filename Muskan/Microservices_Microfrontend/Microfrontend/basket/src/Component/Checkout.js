import React, { useState, useEffect } from "react";
import {
  Container,
  Grid,
  Typography,
  Box,
  Card,
  CardContent,
  Badge,
  List,
  ListItem,
  ListItemText,
  TextField,
  MenuItem,
  Button,
  CircularProgress,
  InputAdornment,
} from "@mui/material";
import ShoppingCartIcon from "@mui/icons-material/ShoppingCart";
import { CheckoutBasket } from "../Services/BasketService";
import { useLocation } from "react-router-dom";
import { useAuth } from "auth/useAuth";

const Checkout = () => {
  const { isAuthenticated, user, getAccessTokenSilently } = useAuth();
  const [isLoading, setIsLoading] = useState(false);
  const { state } = useLocation();
  const cart = state?.cart;

  const [token, setToken] = useState("");
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    address1: "",
    state: "",
    zip: "",
    country: "India",
    cardName: "",
    cardNumber: "",
    expiryDate: "",
    cvv: "",
    paymentMethod: 1,
    username: user?.nickname || user?.name || user?.email || "guestUser",
  });

  useEffect(() => {
    const fetchToken = async () => {
      const t = await getAccessTokenSilently();
      setToken(t);
    };
    fetchToken();
  }, [getAccessTokenSilently]);

  if (!cart) {
    return <div>Cart data is not available</div>;
  }
  console.log(cart);
  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleCheckout = async (e) => {
    e.preventDefault();

    // const checkoutRequest = {
    //   userName: user?.name,
    //   customerId: user?.sub || "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    //   firstName: formData.firstName,
    //   lastName: formData.lastName,
    //   emailAddress: formData.email,
    //   address: formData.address1,
    //   country: formData.country,
    //   state: formData.state,
    //   zip: formData.zip,
    //   totalPrice: cart.totalPrice,
    //   items: cart.items.map((item) => ({
    //     productId: item.productId,
    //     quantity: item.quantity,
    //     price: item.price,
    //   })),
    // };
    const checkoutRequest = {
      basketCheckOutDto: {
        username: user?.nickname || user?.name || user?.email || "guestUser",
        customerId: "58c49479-ec65-4de2-86e7-033c546291aa",
        totalPrice: cart.totalPrice,
        firstName: formData.firstName,
        lastName: formData.lastName,
        emailAdress: formData.email,
        adressLine: formData.address1,
        country: formData.country,
        state: formData.state,
        zipCode: formData.zip,
        cardName: formData.cardName,
        cardNumber: formData.cardNumber,
        expiryDate: formData.expiryDate,
        cvv: formData.cvv,
        paymentMethod: parseInt(formData.paymentMethod),
      },
    };

    console.log("Checkout request", JSON.stringify(checkoutRequest, null, 2));

    console.log("checkoutRequest :", checkoutRequest);

    try {
      const response = await CheckoutBasket(checkoutRequest, token);
      setIsLoading(true);
      console.log(response);
      if (response.data) {
        setIsLoading(false);
      }
      alert("Checkout successful!");
    } catch (error) {
      setIsLoading(false);
      alert("Checkout failed. Please try again.");
      console.error("Checkout error:", error);
    }
  };
  if (isLoading) {
    return <CircularProgress />;
  }

  return (
    <Container>
      <Box my={4}>
        <Typography variant="h6">Your Cart</Typography>
        <Card>
          <CardContent>
            <Badge badgeContent={cart.items.length} color="secondary">
              <ShoppingCartIcon />
            </Badge>
            <List>
              {cart.items.map((item, index) => (
                <ListItem key={index}>
                  <ListItemText
                    primary={item.productName}
                    secondary={`Quantity: ${item.quantity}`}
                  />
                  <Typography>${item.price.toFixed(2)}</Typography>
                </ListItem>
              ))}
              <ListItem>
                <ListItemText primary="Total" />
                <Typography variant="h6">
                  ${cart.totalPrice.toFixed(2)}
                </Typography>
              </ListItem>
            </List>
          </CardContent>
        </Card>
      </Box>

      <form onSubmit={handleCheckout}>
        {/* Form fields */}
        <TextField
          name="firstName"
          label="First Name"
          value={formData.firstName}
          onChange={handleChange}
          fullWidth
          required
        />
        <TextField
          name="lastName"
          label="Last Name"
          value={formData.lastName}
          onChange={handleChange}
          fullWidth
          required
        />
        <TextField
          name="email"
          label="Email"
          value={formData.email}
          onChange={handleChange}
          fullWidth
          required
        />
        <TextField
          name="address1"
          label="Address"
          value={formData.address1}
          onChange={handleChange}
          fullWidth
          required
        />
        <TextField
          name="state"
          label="State"
          value={formData.state}
          onChange={handleChange}
          fullWidth
          required
        />
        <TextField
          name="zip"
          label="Zip Code"
          value={formData.zip}
          onChange={handleChange}
          fullWidth
          required
        />
        <TextField
          name="cardName"
          label="Card Name"
          value={formData.cardName}
          onChange={handleChange}
          fullWidth
          required
        />
        <TextField
          name="cardNumber"
          label="Card Number"
          value={formData.cardNumber}
          onChange={handleChange}
          fullWidth
          required
        />
        <TextField
          name="expiryDate"
          label="Expiry Date"
          value={formData.expiryDate}
          onChange={handleChange}
          fullWidth
          required
        />
        <TextField
          name="cvv"
          label="CVV"
          value={formData.cvv}
          onChange={handleChange}
          fullWidth
          required
        />
        <TextField
          name="paymentMethod"
          label="Payment Method"
          select
          value={formData.paymentMethod}
          onChange={handleChange}
          fullWidth
          required
        >
          <MenuItem value={1}>Credit Card</MenuItem>
          <MenuItem value={2}>Debit Card</MenuItem>
          <MenuItem value={3}>UPI</MenuItem>
          <MenuItem value={4}>Net Banking</MenuItem>
        </TextField>

        <Button type="submit" variant="contained" color="primary">
          Confirm & Pay
        </Button>
      </form>
    </Container>
  );
};

export default Checkout;

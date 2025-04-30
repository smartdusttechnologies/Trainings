import React, { useEffect, useState } from "react";
import {
  Container,
  Typography,
  Breadcrumbs,
  Link,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Button,
  Avatar,
  Alert,
} from "@mui/material";
import HomeIcon from "@mui/icons-material/Home";
import ShoppingCartIcon from "@mui/icons-material/ShoppingCart";
import { getOrdersByCustomer } from "../Services/OrderService";
import { useAuth } from "auth/useAuth";
const OrderList = () => {
  const [orders, setOrders] = useState([]);
  const [error, setError] = useState("");
  const { getAccessTokenSilently, user } = useAuth();

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        console.info("OrderList component is visited");
        const customerId = "58c49479-ec65-4de2-86e7-033c546291aa";
        const token = await getAccessTokenSilently();
        console.log(token);
        const res = await getOrdersByCustomer(customerId, token);
        setOrders(res.orders);
      } catch (error) {
        console.error("Error fetching orders:", error);
        if (error.name === "AbortError") {
          setError("The request took too long. Please try again later.");
        } else {
          setError("Unable to fetch orders. Please try again later.");
        }
      }
    };

    fetchOrders();
  }, []);

  return (
    <Container maxWidth="lg" sx={{ mt: 4 }}>
      <Breadcrumbs aria-label="breadcrumb" sx={{ mb: 2 }}>
        <Link color="inherit" href="/" underline="hover">
          <HomeIcon sx={{ mr: 0.5 }} fontSize="inherit" /> Home
        </Link>
        <Typography color="text.primary">
          <ShoppingCartIcon sx={{ mr: 0.5 }} fontSize="inherit" />
          Order
        </Typography>
      </Breadcrumbs>

      <Typography variant="h4" gutterBottom>
        Your Orders
      </Typography>

      {error && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              {/* <TableCell></TableCell> */}
              <TableCell>Username Name</TableCell>
              <TableCell>First Name</TableCell>
              <TableCell>Last Name</TableCell>
              <TableCell>Email</TableCell>
              <TableCell>Address Line</TableCell>
              <TableCell>Card Name</TableCell>
              <TableCell>Status</TableCell>
              <TableCell align="right">Total Price ($)</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {orders.map((order, index) => (
              <TableRow key={index}>
                {/* <TableCell> */}
                {/* <Avatar src="https://dummyimage.com/50x50/55595c/fff" /> */}
                {/* </TableCell> */}
                <TableCell>{order.orderName}</TableCell>
                <TableCell>{order.shippingAddress.firstname}</TableCell>
                <TableCell>{order.shippingAddress.lastname}</TableCell>
                <TableCell>{order.shippingAddress.emailAdress}</TableCell>
                <TableCell>{order.shippingAddress.addressLine}</TableCell>
                <TableCell>{order.payment.cardName}</TableCell>
                <TableCell>{order.status}</TableCell>
                <TableCell align="right">
                  {order.orderItems
                    .reduce(
                      (total, item) => total + item.price * item.quantity,
                      0
                    )
                    .toFixed(2)}
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Button
        variant="contained"
        color="success"
        size="large"
        sx={{ mt: 3 }}
        href="/"
      >
        Continue Shopping
      </Button>
    </Container>
  );
};

export default OrderList;

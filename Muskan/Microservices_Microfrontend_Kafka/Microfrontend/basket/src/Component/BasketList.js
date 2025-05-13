import React, { useEffect, useState } from "react";
import { useAuth } from "auth/useAuth";
import { useNavigate } from "react-router-dom";
import { GetBasket } from "../Services/BasketService";
import { loadBasket } from "../Utils/LoadBasket";
import {
  Card,
  CardContent,
  Typography,
  Divider,
  CircularProgress,
  List,
  ListItem,
  ListItemText,
  ListItemSecondaryAction,
  IconButton,
  Button,
} from "@mui/material";
import ShoppingCartIcon from "@mui/icons-material/ShoppingCart";

export default function BasketList() {
  const navigate = useNavigate();
  const { getAccessTokenSilently, user, isAuthenticated, isLoading } =
    useAuth();
  const [items, setItems] = useState([]);
  const [totalPrice, setTotalPrice] = useState(0);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchBasket = async () => {
      if (!isAuthenticated || isLoading || !user) return;
      try {
        const token = await getAccessTokenSilently();
        const username =
          user?.nickname || user?.name || user?.email || "guestUser";

        // Using the loadBasket function for fetching basket data
        const basketData = await loadBasket(user, getAccessTokenSilently);

        setItems(basketData.items);
        console.log(basketData);

        setTotalPrice(basketData.totalPrice);
      } catch (err) {
        
  if (
    err.response &&
    err.response.status === 404 &&
    err.response.data?.detail?.includes("Basket")
  ) {
    setError("You don't have a basket yet. Start shopping to add items!");
  } else {
    setError("Failed to load basket. Please try again later.");
  }

        console.error("Error fetching basket:", err);
        setError("Failed to load basket.");
      } finally {
        setLoading(false);
      }
    };

    fetchBasket();
  }, [isAuthenticated, isLoading, user, getAccessTokenSilently]);
  const handleCheckout = () => {
    const cartData = {
      items: items,
      totalPrice: totalPrice,
    };
    console.log(cartData);
    navigate("/basket/checkout", { state: { cart: cartData } });
  };

  return (
    <Card sx={{ maxWidth: 600, margin: "auto", padding: 2, boxShadow: 3 }}>
      <CardContent>
        <Typography variant="h5" component="div" align="center" gutterBottom>
          <ShoppingCartIcon sx={{ mr: 1 }} />
          Your Shopping Basket
        </Typography>

        {loading ? (
          <CircularProgress
            color="primary"
            sx={{ display: "block", margin: "auto", marginTop: 3 }}
          />
        ) : (
          <>
            {error && (
              <Typography color="error" align="center">
                {error}
              </Typography>
            )}

            {items.length === 0 ? (
              <Typography color="textSecondary" align="center">
                Your basket is empty.
              </Typography>
            ) : (
              <>
                <List sx={{ paddingTop: 0 }}>
                  {items.map((item, index) => (
                    <ListItem
                      key={index}
                      sx={{
                        padding: 2,
                        backgroundColor: "background.paper",
                        borderRadius: 2,
                        boxShadow: 1,
                        marginBottom: 2,
                      }}
                    >
                      <ListItemText
                        primary={item.productName}
                        secondary={`Color: ${item.color} | Qty: ${item.quantity}`}
                      />
                      <ListItemSecondaryAction>
                        <Typography variant="body1" color="primary">
                          ₹ {(item.price * item.quantity).toFixed(2)}
                        </Typography>
                      </ListItemSecondaryAction>
                    </ListItem>
                  ))}
                </List>

                <Divider sx={{ marginY: 2 }} />

                <Typography
                  variant="h6"
                  color="success.main"
                  align="right"
                  fontWeight="bold"
                >
                  Total: ₹ {totalPrice.toFixed(2)}
                </Typography>
                <Button
                  variant="contained"
                  color="primary"
                  fullWidth
                  onClick={handleCheckout}
                  sx={{ mt: 2 }}
                >
                  Proceed to Checkout
                </Button>
              </>
            )}
          </>
        )}
      </CardContent>
    </Card>
  );
}

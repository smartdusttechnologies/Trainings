import React, { useState, useEffect } from "react";
import { StoreBasket, GetBasket } from "../Services/BasketService";
import { useAuth } from "auth/useAuth";
import { Button, CircularProgress } from "@mui/material";
import ShoppingCartIcon from "@mui/icons-material/ShoppingCart";

export default function AddToCart({ product }) {
  const [added, setAdded] = useState(false);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const [token, setToken] = useState("");
  const { getAccessTokenSilently, user } = useAuth();

  console.log("Product object passed to AddToCart:", product);

  useEffect(() => {
    const fetchToken = async () => {
      try {
        const _token = await getAccessTokenSilently();
        console.log("Access Token:", _token);
        setToken(_token);
      } catch (err) {
        setError("Unable to get access token");
      }
    };
    fetchToken();
  }, []);

  const handleAdd = async () => {
    if (!token) {
      setError("Token not available. Please log in.");
      return;
    }

    try {
      setLoading(true);
      const username =
        user?.nickname || user?.name || user?.email || "guestUser";

      // Try to get the current basket
      let currentItems = [];
      try {
        const basketRes = await GetBasket(username, token);
        currentItems = basketRes?.cart?.items || [];
      } catch (err) {
        // Handle basket not found (e.g., 404 error)
        if (err?.response?.status === 404) {
          console.warn("Basket not found. Initializing a new basket.");
          currentItems = []; // Start a new cart
        } else if (err?.response?.status === 500) {
          console.warn("Basket not found. Initializing a new basket.");
          currentItems = [];
        } else {
          console.error(
            "Unexpected error fetching basket:",
            err?.message || err
          );
        }
      }

      // Normalize new item
      const newItem = {
        productId: product.id,
        productName: product.name,
        quantity: 1,
        color: "default",
        price: product.price,
      };

      // Merge logic (check if already in cart)
      const itemIndex = currentItems.findIndex(
        (i) => i.productId === newItem.productId
      );
      if (itemIndex > -1) {
        currentItems[itemIndex].quantity += 1;
      } else {
        currentItems.push(newItem);
      }

      // Store basket again
      const body = {
        cart: {
          userName: username,
          items: currentItems,
        },
      };
      console.log("currentItems", currentItems);
      let res = await StoreBasket(currentItems, username, token);
      console.log("Response from StoreBasket:", res);

      if (res.status === 201 || res.status === 200) {
        setAdded(true);
      }
    } catch (err) {
      console.error(err);
      setError("Could not add to cart");
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <Button
        variant="contained"
        color="primary"
        startIcon={<ShoppingCartIcon />}
        onClick={handleAdd}
        disabled={loading}
        sx={{
          marginTop: 2,
          padding: "8px 16px",
          textTransform: "none",
          fontWeight: 600,
          backgroundColor: loading ? "gray" : "#4CAF50",
          "&:hover": {
            backgroundColor: loading ? "gray" : "#45a049",
          },
        }}
      >
        {added ? (
          "Added!"
        ) : loading ? (
          <CircularProgress size={24} color="inherit" />
        ) : (
          "Add to Cart"
        )}
      </Button>
      {error && <p style={{ color: "red", marginTop: "10px" }}>{error}</p>}
    </>
  );
}

import React, { useState, useEffect } from "react";
// import { getProduct } from "../S";
import { getProduct } from "../Services/ProductService";
import { Link } from "react-router-dom";
// import AddToCart from "basket/AddToCart";
// import SafeComponents from "./SafeComponents";

import {
  Container,
  Grid,
  Card,
  CardMedia,
  CardContent,
  Typography,
  Button,
  CircularProgress,
  Box,
} from "@mui/material";
import ShoppingCartIcon from "@mui/icons-material/ShoppingCart";
import AddToCart from "./AddToCart";

export default function ProductList() {
  const [products, setProducts] = useState([]);

  useEffect(() => {
    getProduct().then((data) => {
      console.log("Fetched Products:", data);
      if (data.length > 0) {
        console.log("Image File of first product:", data[0].imageFile);
      }
      setProducts(data);
    });
  }, []);

  if (products.length === 0) {
    return (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        minHeight="100vh"
        bgcolor="#f7f7f7"
      >
        <Box textAlign="center">
          <CircularProgress />
          <Typography variant="h6" mt={2}>
            Loading...
          </Typography>
          <Typography color="textSecondary">
            Please wait while we fetch the products.
          </Typography>
        </Box>
      </Box>
    );
  }

  return (
    <Container sx={{ py: 6 }}>
      <Typography variant="h4" align="center" gutterBottom>
        Product List
      </Typography>
      <Grid container spacing={4}>
        {products.map((product) => (
          <Grid item key={product.id} xs={12} sm={6} md={4}>
            <Card sx={{ borderRadius: 3, boxShadow: 3 }}>
              <Link
                to={`/product/${product.id}`}
                style={{ textDecoration: "none" }}
              >
                <CardMedia
                  component="img"
                  height="200"
                  image={
                    product.imageFile
                      ? `/product/${product.imageFile}`
                      : "https://placehold.jp/150x150.png"
                  }
                  alt={product.name}
                  onError={(e) => {
                    e.currentTarget.src = "https://placehold.jp/150x150.png";
                  }}
                />
              </Link>
              <CardContent>
                <Link
                  to={`/product/${product.id}`}
                  style={{ textDecoration: "none" }}
                >
                  <Typography variant="h6" color="textPrimary">
                    Name: {product.name}
                  </Typography>
                </Link>
                <Typography variant="body2" color="textSecondary" mt={1}>
                  {product.description}
                </Typography>
                <Typography variant="h6" color="green" mt={2}>
                  ₹ {product.price}
                </Typography>
                <Box mt={2}>
                  {/* <SafeComponents> */}
                  <Button
                    // variant="contained"
                    // color="success"
                    // fullWidth
                    // startIcon={<ShoppingCartIcon />}
                    sx={{ borderRadius: 2 }}
                  >
                    <AddToCart product={product} />
                  </Button>
                  {/* </SafeComponents> */}
                </Box>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Container>
  );
}

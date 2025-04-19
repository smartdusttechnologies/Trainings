import React, { useState, useEffect } from "react";
import { getProductById } from "../Services/ProductService";
import { useParams } from "react-router-dom";
import {
  Box,
  CircularProgress,
  Container,
  Typography,
  Grid,
  Card,
  CardMedia,
  CardContent,
  Button,
  Chip,
} from "@mui/material";
import ShoppingCartIcon from "@mui/icons-material/ShoppingCart";

const IMAGE_BASE_URL = "http://localhost:3000/product/";

export default function ProductDetails() {
  const { id } = useParams();
  const [product, setProduct] = useState(null);

  useEffect(() => {
    if (!id) return;

    const fetchProduct = async () => {
      try {
        console.log("Calling API with id:", id);
        const response = await getProductById(id);
        console.log("API response:", response);
        setProduct(response.products);
      } catch (error) {
        console.error("Error fetching product:", error);
      }
    };

    fetchProduct();
  }, [id]);

  if (!product) {
    return (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        minHeight="100vh"
        bgcolor="#f5f5f5"
      >
        <Box textAlign="center">
          <CircularProgress />
          <Typography variant="h6" mt={2}>
            Loading...
          </Typography>
          <Typography color="textSecondary">
            Fetching product details...
          </Typography>
        </Box>
      </Box>
    );
  }

  return (
    <Container sx={{ py: 6 }}>
      <Card sx={{ p: 4, borderRadius: 3, boxShadow: 4 }}>
        <Grid container spacing={4}>
          <Grid item xs={12} md={6}>
            <CardMedia
              component="img"
              height="400"
              image={
                product.imageFile
                  ? `/product/${product.imageFile}`
                  : "https://placehold.co/400x300?text=No+Image"
              }
              alt={product.name}
              onError={(e) => {
                e.currentTarget.src =
                  "https://placehold.co/400x300?text=Image+Missing";
              }}
              sx={{ borderRadius: 2 }}
            />
          </Grid>

          <Grid
            item
            xs={12}
            md={6}
            display="flex"
            flexDirection="column"
            justifyContent="space-between"
          >
            <CardContent>
              <Chip
                label={product.category?.[0] || "Uncategorized"}
                color="primary"
                variant="outlined"
                sx={{ mb: 2 }}
              />
              <Typography variant="h4" fontWeight="bold" gutterBottom>
                {product.name}
              </Typography>
              <Typography variant="body1" color="text.secondary" paragraph>
                {product.description}
              </Typography>
              <Typography
                variant="h5"
                color="green"
                fontWeight="bold"
                gutterBottom
              >
                ₹ {product.price}
              </Typography>
            </CardContent>

            <Box px={2} pb={2}>
              <Button
                variant="contained"
                color="primary"
                size="large"
                startIcon={<ShoppingCartIcon />}
                sx={{ borderRadius: 2 }}
              >
                Buy Now
              </Button>
            </Box>
          </Grid>
        </Grid>
      </Card>
    </Container>
  );
}

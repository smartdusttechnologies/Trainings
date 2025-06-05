import axios from "axios";

const API_SERVER = "https://localhost:6064/catalog-service";

// Get all products
export const getProduct = async () => {
  try {
    const response = await axios.get(`${API_SERVER}/products`);
    return response.data.products;
  } catch (error) {
    console.error("Error fetching products:", error);
    throw error; // Re-throw if you want to handle it further up
  }
};

// Get product by ID
export const getProductById = async (id) => {
  try {
    const response = await axios.get(`${API_SERVER}/products/${id}`);
    return response.data;
  } catch (error) {
    console.error(`Error fetching product with ID ${id}:`, error);
    throw error;
  }
};

import axios from "axios";

const API_SERVER = "https://localhost:6064/basket-service";

export const StoreBasket = (items, username, token) => {
  if (!Array.isArray(items) || items.length === 0) {
    throw new Error("Invalid items passed to StoreBasket");
  }

  const body = {
    cart: {
      userName: username,
      items: items.map((item) => ({
        quantity: item.quantity,
        color: item.color || "default",
        price: item.price,
        productId: item.productId,
        productName: item.productName,
      })),
    },
  };

  return axios.post(`${API_SERVER}/basket`, body, {
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  });
};

export async function GetBasket(username, token) {
  try {
    const response = await axios.get(`${API_SERVER}/basket/${username}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    return response.data;
  } catch (error) {
    console.error("Axios error fetching basket:", error);
    throw new Error("Failed to fetch basket");
  }
}

export async function CheckoutBasket(checkoutRequest, token) {
  try {
    const response = await axios.post(
      `${API_SERVER}/basket/checkout`,
      checkoutRequest,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return response;
  } catch (error) {
    console.error("Error during basket checkout:", error);
    throw error;
  }
}

import { GetBasket } from "../Services/BasketService.js";

// Equivalent to  LoadUserBasket
export async function loadBasket(user, getAccessTokenSilently) {
  let basket;

  if (!user) {
    return {
      userName: "guestUser",
      items: [],
      totalPrice: 0,
    };
  }

  const username = user.nickname || user.name || user.email || "guestUser";

  try {
    const token = await getAccessTokenSilently();

    // Call GetBasket with token (like authorized API call)
    const response = await GetBasket(username, token);

    basket = response.cart;
  } catch (error) {
    if (error.response) {
      const status = error.response.status;

      if (status === 404) {
        // User not found => return empty cart
        basket = {
          userName: username,
          items: [],
          totalPrice: 0,
        };
      } else if (status === 401) {
        // Unauthorized
        throw new Error("You are Unauthorized");
      } else {
        throw new Error("Unhandled API error: " + error.message);
      }
    } else {
      // Network error or unexpected failure
      throw new Error("Unexpected error: " + error.message);
    }
  }

  return basket;
}

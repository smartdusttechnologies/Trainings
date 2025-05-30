import axios from "axios";

const API_SERVER = "http://localhost:6004/ordering-service";
export const getOrdersByCustomer = async (customerId, token) => {
  console.info("OrderList component is visited");
  const response = await axios.get(
    `${API_SERVER}/orders/customer/${customerId}`,
    {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    }
  );
  console.log(response);
  return response.data;
};

import React from "react";
import RestaurantNotification from "./RestaurantNotification";

const HandleRestaurantButton = ({
  handleAcceptOrder,
  handleDeclineOrder,
  confirmOrderPreparation,
  order,
  riderLat,
  riderLng,
  userLat,
  userLng,
  restaurantLat,
  restaurantLng,
}) => {
  return (
    <div>
      <p>
        <strong>Order ID:</strong> {order.orderId}
      </p>
      <p>
        <strong>User:</strong> {order.userId}
      </p>
      <p>
        <strong>Status:</strong> {order.status}
      </p>
      {riderLat && riderLng && riderLat !== 0 && riderLng !== 0 && (
        <p>
          <strong>
            Rider Location: {riderLat} and {riderLng}
          </strong>
        </p>
      )}
      {userLat && userLng && userLat !== 0 && userLng !== 0 && (
        <p>
          <strong>
            User Location: {userLat} and {userLng}
          </strong>
        </p>
      )}

      <p>
        <strong>Order Items:</strong> {order.orderItems}
      </p>
      {order.riderId && (
        <p>
          <strong>Rider Assigned:</strong> {order.riderId}
        </p>
      )}
      <button
        className="btn btn-success m-2"
        onClick={() =>
          handleAcceptOrder(
            order.orderId,
            order.userId,
            order.userLat,
            order.userLng
          )
        }
      >
        Accept
      </button>
      <button
        className="btn btn-danger m-2"
        onClick={() =>
          handleDeclineOrder(
            order.orderId,
            order.userId,
            order.userLat,
            order.userLng
          )
        }
      >
        Decline
      </button>
      <button
        className="btn btn-primary m-2"
        onClick={() =>
          confirmOrderPreparation(
            order.orderId,
            order.userId,
            order.userLat,
            order.userLng
          )
        }
      >
        Confirm Preparation
      </button>
    </div>
  );
};

export default HandleRestaurantButton;

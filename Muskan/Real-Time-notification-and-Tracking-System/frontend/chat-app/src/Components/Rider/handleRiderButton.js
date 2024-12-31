import React from "react";

const HandleRiderButton = ({
  order,
  handleAcceptOrder,
  handleDeclineOrder,
  handleDeliveryAcceptFromRestaurant,
  handleSuccessDelivery,
}) => {
  return (
    <div>
      <h5 className="card-title">Order ID: {order.orderId}</h5>
      <p className="card-text">
        <strong>Status:</strong>{" "}
        <span
          className={`badge ${
            order.status === "New Delivery Order" ? "bg-warning" : "bg-success"
          }`}
        >
          {order.status}
        </span>
      </p>
      <p className="card-text">
        <strong>Restaurant Location:</strong> {order.restaurantLat},{" "}
        {order.restaurantLng}
      </p>
      <p className="card-text">
        <strong>User Location:</strong> {order.userLat}, {order.userLng}
      </p>
      <p className="card-text">
        <strong>Restaurant Id</strong> {order.restaurantId}
      </p>
      <button
        className="btn btn-success"
        onClick={() =>
          handleAcceptOrder(order.orderId, order.userId, order.restaurantId)
        }
      >
        <i className="fas fa-check"></i> Accept Order
      </button>

      <button
        className="btn btn-danger"
        onClick={() => handleDeclineOrder(order.orderId)}
      >
        <i className="fas fa-times"></i> Decline Order
      </button>

      {/* <button
        className="btn btn-primary"
        onClick={() =>
          handleDeliveryAcceptFromRestaurant(
            order.orderId,
            order.userId,
            order.restaurantId
          )
        }
      >
        <i className="fas fa-check-circle"></i> Order Received from Restaurant
      </button>

      <button
        className="btn btn-primary"
        y
        onClick={() =>
          handleSuccessDelivery(order.orderId, order.userId, order.restaurantId)
        }
      >
        <i className="fas fa-check-circle"></i> Order Delivered Successfully
      </button> */}
    </div>
  );
};

export default HandleRiderButton;

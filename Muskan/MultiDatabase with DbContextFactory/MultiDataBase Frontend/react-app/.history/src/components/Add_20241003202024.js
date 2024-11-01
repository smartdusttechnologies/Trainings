import React from "react";
import "./style.css";

const Add = () => {
  return (
    <>
      <form className="form">
        <h2>Add a new user</h2>
        <label>
          Name:
          <input
            className="form-input"
            id="user_input"
            type="text"
            value="some value"
            name="name"
          />
        </label>
        <label>
          Address:
          <input
            className="form-input"
            id="address_input"
            type="text"
            value="some value"
            name="address"
          />
        </label>
        <input
          type="submit"
          id="button_submit"
          className="btn"
          value="Submit"
        />
      </form>
    </>
  );
};

export default Add;

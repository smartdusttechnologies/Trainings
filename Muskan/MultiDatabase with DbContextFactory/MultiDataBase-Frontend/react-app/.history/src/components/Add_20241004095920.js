import React from "react";
import "./style.css";

const Add = ({ handleSubmit, setAddress, setName }) => {
  return (
    <>
      <form className="form" onSubmit={handleSubmit}>
        <h2>Add a new user</h2>
        <label>
          Name:
          <input
            className="form-input"
            id="user_input"
            onChange={(event) => setName(event.target.value)}
            type="text"
            name="name"
          />
        </label>
        <label>
          Address:
          <input
            className="form-input"
            onChange={(event) => setAddress(event.value.target)}
            id="address_input"
            type="text"
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

import React, { useEffect, useState } from "react";
import "./style.css";

const Add = (handleSubmit) => {
  return (
    <>
      <form className="form" onSubmit={handleSubmit}>
        <h2>Add a new user</h2>
        <label>
          Name:
          <input
            className="form-input"
            id="user_input"
            onChange={(e) => setName(e.value.target)}
            type="text"
            value="some value"
            name="name"
          />
        </label>
        <label>
          Address:
          <input
            className="form-input"
            onChange={(e) => setAddress(e.value.target)}
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

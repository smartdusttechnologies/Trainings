import React from "react";
import "./style.css";

const Add = () => {
  return (
    <>
      <form className="form">
        <h2>Add a new user</h2>
        <label>
          Name:
          <input className="form-input" type="text" name="name" />
        </label>
        <label>
          Address:
          <input className="form-input" type="text" name="address" />
        </label>
        <input type="submit" className="btn" value="Submit" />
      </form>
    </>
  );
};

export default Add;

import React from "react";
import "./style.css";

const Add = ({
  handleSubmit,
  setAddress,
  setName,
  setDesignation,
  setSalary,
  setFile,
}) => {
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
            onChange={(event) => setAddress(event.target.value)}
            id="address_input"
            type="text"
            name="address"
          />
        </label>
        <label>
          Designation:
          <input
            className="form-input"
            onChange={(e) => setDesignation(e.target.value)}
            type="text"
            id="designation_input"
            name="designation"
          />
        </label>
        <label>
          Salary:
          <input
            className="form-input"
            onChange={(e) => setSalary(e.target.value)}
            type="number"
            id="salary_input"
            name="salary"
          />
        </label>
        <label>
          File Upload :
          <input
            className="form-input"
            type="file"
            onChange={handleFileChange} // Handle file change
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

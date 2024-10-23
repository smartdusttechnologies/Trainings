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
    <form className="form" onSubmit={handleSubmit}>
      <h2>Add a new user</h2>
      <label>
        Name:
        <input
          className="form-input"
          onChange={(event) => setName(event.target.value)}
          type="text"
          name="name"
          required
        />
      </label>
      <label>
        Address:
        <input
          className="form-input"
          onChange={(event) => setAddress(event.target.value)}
          type="text"
          name="address"
          required
        />
      </label>
      <label>
        Designation:
        <input
          className="form-input"
          onChange={(e) => setDesignation(e.target.value)}
          type="text"
          name="designation"
          required
        />
      </label>
      <label>
        Salary:
        <input
          className="form-input"
          onChange={(e) => setSalary(e.target.value)}
          type="number"
          name="salary"
          required
        />
      </label>
      <label>
        File Upload:
        <input
          className="form-input"
          type="file"
          onChange={(e) => setFile(e.target.files[0])} // Correctly set the file object
          required
        />
      </label>
      <input type="submit" className="btn" value="Submit" />
    </form>
  );
};

export default Add;

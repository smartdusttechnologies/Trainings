import React from "react";
import "./style.css";

const Add = ({
  handleSubmit,
  setHomeAddress,
  setName,
  setDesignation,
  setEmployeeSurname,
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
          id="name-input"
          required
        />
      </label>
      <label>
        Address:
        <input
          className="form-input"
          onChange={(event) => setHomeAddress(event.target.value)}
          type="text"
          name="address"
          id="address-input"
          required
        />
      </label>
      <label>
        Designation:
        <input
          className="form-input"
          onChange={(e) => setDesignation(e.target.value)}
          type="text"
          id="designation-input"
          name="designation"
          required
        />
      </label>
      <label>
        EmployeeSurname:
        <input
          className="form-input"
          onChange={(e) => setEmployeeSurname(e.target.value)}
          type="number"
          name="EmployeeSurname"
          id="EmployeeSurname-input"
          required
        />
      </label>
      <label>
        File Upload:
        <input
          className="form-input"
          type="file"
          id="file-input"
          onChange={(e) => setFile(e.target.files[0])} //
          required
        />
      </label>
      <input type="submit" className="btn" value="Submit" />
    </form>
  );
};

export default Add;

// Edit.js
import React from "react";

const Edit = ({ handleUpdate, editData, setEditData }) => {
  const handleChange = (e) => {
    const { name, value } = e.target;
    setEditData((prevData) => ({ ...prevData, [name]: value })); // Update editData state
  };

  return (
    <div>
      <form
        className="form"
        id="form-edit"
        onSubmit={(e) => {
          e.preventDefault();
          handleUpdate(editData.id); // Pass the ID to the update handler
        }}
      >
        <h2>Edit User</h2>
        <label>
          Name:
          <input
            className="form-input"
            type="text"
            name="name"
            id="uname"
            value={editData.name || ""}
            onChange={handleChange}
            placeholder="Name"
          />
        </label>
        <label>
          Address:
          <input
            className="form-input"
            type="text"
            name="address"
            id="uaddress"
            value={editData.address || ""}
            onChange={handleChange}
            placeholder="Address"
          />
        </label>
        <label>
          Designation:
          <input
            className="form-input"
            type="text"
            name="designation"
            id="udesignation"
            value={editData.designation || ""}
            onChange={handleChange}
            placeholder="Designation"
          />
        </label>

        <label>
          Salary:
          <input
            className="form-input"
            type="number"
            name="salary"
            id="usalary"
            value={editData.salary || ""}
            onChange={handleChange}
            placeholder="Salary"
          />
        </label>

        <button type="submit" className="btn">
          Update
        </button>
      </form>
    </div>
  );
};

export default Edit;

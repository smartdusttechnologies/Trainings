// Edit.js
import React from "react";

const Edit = ({ handleUpdate, editData, setEditData }) => {
  const handleChange = (e) => {
    const { name, value } = e.target;
    setEditData((prevData) => ({ ...prevData, [name,address,]: value })); // Update editData state
  };

  return (
    <div>
      <form
        className="form"
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
            value={editData.address || ""}
            onChange={handleChange}
            placeholder="Address"
          />
        </label>

        <input
          type="text"
          name="designation"
          value={editData.designation || ""}
          onChange={handleChange}
          placeholder="Designation"
        />
        <input
          type="number"
          name="salary"
          value={editData.salary || ""}
          onChange={handleChange}
          placeholder="Salary"
        />
        <button type="submit">Update User</button>
      </form>
    </div>
  );
};

export default Edit;

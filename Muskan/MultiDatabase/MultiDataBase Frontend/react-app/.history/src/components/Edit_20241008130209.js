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
          handleUpdate(editData.id);
        }}
      >
        <h2>Edit User</h2>
        <label>
          Name:
          <input
            className="form-input"
            type="text"
            name="uname"
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
            name="uaddress"
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
            name="udesignation"
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
            name="usalary"
            id="usalary"
            value={editData.salary || ""}
            onChange={handleChange}
            placeholder="Salary"
          />
        </label>

        <button type="submit" id="button_update" className="btn">
          Update
        </button>
      </form>
    </div>
  );
};

export default Edit;

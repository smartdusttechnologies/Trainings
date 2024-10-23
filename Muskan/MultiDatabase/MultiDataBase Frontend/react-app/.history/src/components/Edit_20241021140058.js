import React from "react";

const Edit = ({ handleUpdate, editData, setEditData }) => {
  const handleChange = (e) => {
    const { name, value } = e.target;
    setEditData((prevData) => ({ ...prevData, [name]: value }));
  };

  const handleFileChange = (e) => {
    setEditData((prevData) => ({ ...prevData, file: e.target.files[0] }));
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
            name="name"
            id="uname"
            value={editData.name || ""}
            onChange={handleChange}
            placeholder="Name"
            required
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
            required
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
            required
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
            required
          />
        </label>
        <label>
          File Upload:
          <input
            className="form-input"
            type="file"
            id="ufile"
            name="file"
            onChange={handleFileChange}
            accept=".pdf,.doc,.docx,.jpg,.png"
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

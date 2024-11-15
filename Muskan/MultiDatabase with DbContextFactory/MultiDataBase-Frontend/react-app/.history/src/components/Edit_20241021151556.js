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
            value={editData.name || ""}
            onChange={handleChange}
            required
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
            required
          />
        </label>
        <label>
          Designation:
          <input
            className="form-input"
            type="text"
            name="designation"
            value={editData.designation || ""}
            onChange={handleChange}
            required
          />
        </label>
        <label>
          Salary:
          <input
            className="form-input"
            type="number"
            name="salary"
            value={editData.salary || ""}
            onChange={handleChange}
            required
          />
        </label>
        <label>
          File Upload:
          <input
            className="form-input"
            type="file"
            name="file"
            onChange={handleFileChange}
            accept=".pdf,.doc,.docx,.jpg,.png"
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

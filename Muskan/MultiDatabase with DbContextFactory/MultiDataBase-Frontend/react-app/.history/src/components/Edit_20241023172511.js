import React, { useState } from "react";

const Edit = ({ handleUpdate, editData, setEditData }) => {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");

  const handleChange = (e) => {
    const { name, value } = e.target;
    setEditData((prevData) => ({ ...prevData, [name]: value }));
  };

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (
      file &&
      (file.type === "application/pdf" || file.type.startsWith("image/"))
    ) {
      setEditData((prevData) => ({ ...prevData, file }));
    } else {
      setError("Please upload a valid file (PDF or image).");
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    setError("");

    try {
      await handleUpdate(editData.id);
      // Optionally, show a success message or redirect
    } catch (err) {
      setError("Failed to update user. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div>
      <form className="form" onSubmit={handleSubmit}>
        <h2>Edit User</h2>
        {error && <p className="error">{error}</p>}
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
            name="homeAddress"
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
        {/* <label>
          Salary:
          <input
            className="form-input"
            type="number"
            name="salary"
            value={editData.salary || ""}
            onChange={handleChange}
            required
          />
        </label> */}
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
        <button type="submit" className="btn" disabled={isLoading}>
          {isLoading ? "Updating..." : "Update"}
        </button>
      </form>
    </div>
  );
};

export default Edit;

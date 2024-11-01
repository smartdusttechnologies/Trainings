import React from "react";

const Edit = ({ handleUpdate, editData, setEditData, editId, data }) => {
  const userToEdit = data.find((user) => user.id === editId);

  if (!userToEdit) return null;

  return (
    <form
      className="form"
      onSubmit={(event) => {
        event.preventDefault();
        handleUpdate(editId);
      }}
    >
      <h2>Edit User</h2>
      <label>
        Name:
        <input
          className="form-input"
          onChange={(event) =>
            setEditData({ ...editData, name: event.target.value })
          } // Update name
          type="text"
          name="name"
          value={editData.name}
        />
      </label>
      <label>
        Address:
        <input
          className="form-input"
          onChange={(event) =>
            setEditData({ ...editData, address: event.target.value })
          } // Update address
          type="text"
          name="address"
          value={editData.address}
        />
      </label>
      <label>
        Designation:
        <input
          className="form-input"
          onChange={(event) =>
            setEditData({ ...editData, designation: event.target.value })
          } // Update designation
          type="text"
          name="designation"
          value={editData.designation}
        />
      </label>
      <label>
        Salary:
        <input
          className="form-input"
          onChange={(event) =>
            setEditData({ ...editData, salary: event.target.value })
          } // Update salary
          type="number"
          name="salary"
          value={editData.salary}
        />
      </label>
      <input type="submit" id="button_submit" className="btn" value="Update" />
    </form>
  );
};

export default Edit;

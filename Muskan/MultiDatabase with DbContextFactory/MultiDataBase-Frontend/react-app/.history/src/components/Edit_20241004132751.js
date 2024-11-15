import React from "react";

const Edit = ({ handleUpdate, editData, setEditData, editId }) => {
  return (
    <>
      {data.map((user, index) =>
        user.id === editId ? (
          <form className="form" onSubmit={handleUpdate}>
            <h2>Add a new user</h2>
            <label>
              Name:
              <input
                className="form-input"
                id="user_input"
                onChange={(event) => useName(event.target.value)}
                type="text"
                name="name"
                value={uname}
              />
            </label>
            <label>
              Address:
              <input
                className="form-input"
                onChange={(event) => useAddress(event.target.value)}
                id="address_input"
                type="text"
                name="address"
                value={uaddress}
              />
            </label>
            <label>
              Designation:
              <input
                className="form-input"
                onChange={(event) => useDesignation(event.target.value)}
                type="text"
                name="designation"
                value={designation}
              />
            </label>
            <label>
              Salary:
              <input
                className="form-input"
                onChange={(event) => useSalary(event.target.value)}
                type="number"
                name="salary"
                value={usalary}
              />
            </label>
            <input
              type="submit"
              id="button_submit"
              className="btn"
              value="Update"
              onClick={handleUpdate}
            />
          </form>
        ) : (
          console.log("Can't Edit")
        )
      )}
    </>
  );
};

export default Edit;

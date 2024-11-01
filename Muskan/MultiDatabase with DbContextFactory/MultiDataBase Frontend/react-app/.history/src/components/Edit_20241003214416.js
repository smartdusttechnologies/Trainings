import React from "react";

const Edit = () => {
  return (
    <>
      {data.map((user, index) =>
        user.id === editId ? (
          <form className="form">
            <h2>Edit user</h2>
            <label>
              Name:
              <input
                className="form-input"
                type="text"
                name="name"
                value={user.name}
              />
            </label>
            <label>
              Address:
              <input
                className="form-input"
                value={user.address}
                type="text"
                name="address"
              />
            </label>
            <input type="submit" className="btn" value="Submit" />
          </form>
        ) : (
          console.log("Can't Edit")
        )
      )}
    </>
  );
};

export default Delete;

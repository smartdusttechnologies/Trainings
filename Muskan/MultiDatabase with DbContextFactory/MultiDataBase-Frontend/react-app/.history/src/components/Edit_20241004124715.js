import React from "react";

const Edit = () => {
  return (
    <>
      {data.map((user, index) =>
        user.id === editId ? (
          <form className="form" onSubmit={handleSubmit}>
            <h2>Add a new user</h2>
            <label>
              Name:
              <input
                className="form-input"
                id="user_input"
                onChange={(event) => setName(event.target.value)}
                type="text"
                name="name"
              />
            </label>
            <label>
              Address:
              <input
                className="form-input"
                onChange={(event) => setAddress(event.target.value)}
                id="address_input"
                type="text"
                name="address"
              />
            </label>
            <label>
              Designation:
              <input
                className="form-input"
                onChange={(e) => setDesignation(e.target.value)}
                type="text"
                name="designation"
              />
            </label>
            <label>
              Salary:
              <input
                className="form-input"
                onChange={(e) => setSalary(e.target.value)}
                type="number"
                name="salary"
              />
            </label>
            <input
              type="submit"
              id="button_submit"
              className="btn"
              value="Submit"
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

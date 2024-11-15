import React from "react";

const List = ({ handleEdit, data }) => {
  return (
    <>
      <table className="table">
        <thead className="table-heading">
          <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Address</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {data.map((user) => (
            <tr key={user.id}>
              {" "}
              {/* Using user.id as key */}
              <td>{user.id}</td>
              <td>{user.name}</td>
              <td>{user.address}</td>
              <td className="action">
                <button
                  className="btn1 btn-edi"
                  onClick={() => handleEdit(user.id)}
                  aria-label={`Edit user ${user.name}`} // Accessibility improvement
                >
                  Edit
                </button>
                {/* Uncomment below to add delete functionality later */}
                {/* <button
                  className="btn1 btn-del"
                  onClick={() => handleDelete(user.id)}
                  aria-label={`Delete user ${user.name}`} // Accessibility improvement
                >
                  Delete
                </button> */}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </>
  );
};

export default List;

import React from "react";
const List = ({ handelEdit, data }) => {
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
          {data.map((user, index) => (
            <tr key={index}>
              <td>{user.id}</td>
              <td>{user.name}</td>
              <td>{user.address}</td>
              <td className="action">
                <button
                  className="btn1 btn-edi"
                  onClick={() => handleEdit(user.id)}
                >
                  Edit
                </button>
                {/* <button
                  className="btn1 btn-del"
                  onClick={() => handleDelete(user.id)}
                > */}
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </>
  );
};

export default List;

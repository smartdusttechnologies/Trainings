import React from "react";

const List = ({ data, handleDelete, handleEdit }) => {
  const handleDownload = (fileName) => {
    const url = `http://example.com/files/${fileName}`;
    window.open(url, "_blank");
  };

  return (
    <>
      <table className="table">
        <thead className="table-heading">
          <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Address</th>
            <th>Designation</th>
            <th>Salary</th>
            <th>File</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {data.map((user) => (
            <tr key={user.id} id={`user-row-${user.id}`}>
              <td>{user.id}</td>
              <td>{user.name}</td>
              <td>{user.address}</td>
              <td>{user.designation}</td>
              <td>{user.salary}</td>
              <td>
                {user.file && (
                  <button
                    className="btn1 btn-edi"
                    onClick={() => handleDownload(user.id)}
                  >
                    Download
                  </button>
                )}
              </td>
              <td className="action">
                <button
                  id={`edit-button-${user.id}`}
                  className="btn1 btn-edi"
                  onClick={() => handleEdit(user.id)}
                  aria-label={`Edit user ${user.name}`}
                >
                  Edit
                </button>
                <button
                  name="delete_user"
                  id={`delete-button-${user.id}`}
                  className="btn1 btn-del"
                  onClick={() => handleDelete(user.id)}
                  aria-label={`Delete user ${user.name}`}
                >
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

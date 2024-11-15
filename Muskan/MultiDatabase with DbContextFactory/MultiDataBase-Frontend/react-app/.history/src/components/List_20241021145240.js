import React from "react";

const List = ({ data, handleDelete, handleEdit }) => {
  const handleDownload = (fileName) => {
    // Implement file download logic
    const url = `http://localhost:7122/api/Employee/download/${fileName}`;
    window.open(url, "_blank");
  };

  return (
    <div>
      <h2>Employee List</h2>
      <table className="table">
        <thead>
          <tr>
            <th>Name</th>
            <th>Address</th>
            <th>Designation</th>
            <th>Salary</th>
            <th>File</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {data.map((user) => (
            <tr key={user.id}>
              <td>{user.name}</td>
              <td>{user.address}</td>
              <td>{user.designation}</td>
              <td>{user.salary}</td>
              <td>
                {user.file && (
                  <button onClick={() => handleDownload(user.file)}>
                    Download
                  </button>
                )}
              </td>
              <td>
                <button onClick={() => handleEdit(user.id)}>Edit</button>
                <button onClick={() => handleDelete(user.id)}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default List;

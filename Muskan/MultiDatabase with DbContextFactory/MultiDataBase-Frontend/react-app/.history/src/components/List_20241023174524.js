import React from "react";

const List = ({ data, handleDelete, handleEdit }) => {
  const handleDownload = (fileName) => {
    const url = `https://localhost:7122/api/Employee/{id}/download`;
    window.open(url, "_blank");
  };
  console.log(data);

  return (
    <>
      <table className="table">
        <thead className="table-heading">
          <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Home Address</th>
            <th>Designation</th>
            <th>EmployeeSurname</th>
            <th>File</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {data.map((user) => (
            <tr key={user.id} id={`user-row-${user.id}`}>
              <td>{user.id}</td>
              <td>{user.name}</td>
              <td>{user.homeAddress}</td>
              <td>{user.designation}</td>
              <td>{user.employeeSurname ? user.employeeSurname : "N/A"}</td>
              <td>
                {user.file && (
                  <button
                    className="btn1 btn-edi"
                    onClick={() => handleDownload(user.file)}
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

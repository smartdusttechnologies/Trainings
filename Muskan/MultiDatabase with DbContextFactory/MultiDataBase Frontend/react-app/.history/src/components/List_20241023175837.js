import React from "react";

const List = ({ data, handleDelete, handleEdit }) => {
  const handleDownload = () => {
    const link = document.createElement("a");
    link.href = `data:application/octet-stream;base64,${fileData}`;
    link.download = "filename.extension";
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
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
                    className="download-btn"
                    onClick={() => handleDownload(user.id)}
                    style={{ marginLeft: "10px" }}
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

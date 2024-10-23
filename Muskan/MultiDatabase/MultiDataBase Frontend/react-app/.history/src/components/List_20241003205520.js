import React from "react";
const List = () => {
  return (
    <>
      <table class="table">
        <thead className="table-heading">
          <th>Name</th>
          <th>Address</th>
          {/* <th>Address</th>
          <th>Address</th>
          <th>Address</th> */}
          <th>Action</th>
        </thead>
        <tbody>
          {data.map((user, index) => {
            return (
              <tr key={index}>
                <td>{user.name}</td>
                <td>{user.address}</td>
                {/* <td>{user.address}</td>
                <td>{user.address}</td>
                <td>{user.address}</td> */}
                <td className="action">
                  <button className="btn1 btn-edi">Edit</button>
                  <button className="btn1 btn-del">Delete</button>
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </>
  );
};

export default List;

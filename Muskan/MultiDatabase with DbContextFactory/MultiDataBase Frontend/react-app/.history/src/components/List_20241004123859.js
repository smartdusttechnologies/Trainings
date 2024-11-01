import React, { useEffect, useState } from "react";
const List = ({ handelEdit, data }) => {
  return (
    <>
      <table class="table">
        <thead className="table-heading">
          <th>Id</th>
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
                <td>{user.id}</td>
                <td>{user.name}</td>
                <td>{user.address}</td>
                {/* <td>{user.address}</td>
                <td>{user.address}</td>
                <td>{user.address}</td> */}
                <td className="action">
                  <button
                    className="btn1 btn-edi"
                    onClick={() => handelEdit(user.id)}
                  >
                    Edit
                  </button>
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

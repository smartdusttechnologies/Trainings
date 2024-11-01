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
        <tr>
          <td>Anom</td>
          <td>19</td>
          {/* <td>Male</td>
          <td>Male</td>
          <td>Male</td> */}
          <td className="action">
            <button className="btn1 btn-edi">Edit</button>
            <button className="btn1 btn-del">Delete</button>
          </td>
        </tr>
        <tr>
          <td>Megha</td>
          <td>19</td>
          <td>19</td>
          <td>19</td>
          <td>19</td>
          <td className="action">
            <button className="btn1 btn-edi">Edit</button>
            <button className="btn1 btn-del">Delete</button>
          </td>
        </tr>
        <tr>
          <td>Subham</td>
          <td>Subham</td>
          <td>Subham</td>
          <td>Subham</td>
          <td>25</td>
          <td className="action">
            <button className="btn1 btn-edi">Edit</button>
            <button className="btn1 btn-del">Delete</button>
          </td>
        </tr>
      </table>
    </>
  );
};

export default List;

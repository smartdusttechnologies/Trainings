import React from "react";

const Delete = () => {
  return (
    <>
      <form className="form">
        <h2>Edit user</h2>
        {data.map ((user,index)=>(
          
        ))}
        <label>
          Name:
          <input className="form-input" type="text" name="name" />
        </label>
        <label>
          Address:
          <input className="form-input" type="text" name="address" />
        </label>
        <input type="submit" className="btn" value="Submit" />
      </form>
    </>
  );
};

export default Delete;

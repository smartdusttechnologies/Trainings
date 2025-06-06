import React, { useEffect, useState } from "react";
// import "./App.css";
import { Container, Navbar } from "react-bootstrap";
import Add from "./components/Add";
import List from "./components/List";
// import Delete from "./components/Delete";
import Edit from "./components/Edit";
import axios from "axios";

const App = () => {
  const [data, setData] = useState([]);

  const [name, setName] = useState("");
  const [address, setAddress] = useState("");
  const [designation, setDesignation] = useState("");
  const [salary, setSalary] = useState("");

  const [uname, useName] = useState("");
  const [uaddress, useAddress] = useState("");
  const [udesignation, useDesignation] = useState("");
  const [usalary, useSalary] = useState("");

  const [editData, setEditData] = useState({}); // State for the data being edited
  const [editId, setEditId] = useState(null);

  useEffect(() => {
    axios
      .get("https://localhost:7214/api/Employee")
      .then((res) => setData(res.data))
      .catch((error) => console.error("Error fetching data:", error));
  }, []);
  const handleSubmit = (event) => {
    event.preventDefault();
    // const id = data.length + 1;
    axios
      .post("https://localhost:7214/api/Employee", {
        name: name,
        address: address,
        designation: designation,
        salary: salary,
      })
      .then((response) => {
        console.log("Response:", response.data);
        setData([...data, response.data]);
        setName("");
        setAddress("");
        setDesignation("");
        setSalary("");
        window.location.reload();
      })
      .catch((error) => {
        console.log("Error:", error);
      });
  };

  const handleEdit = (id) => {
    axios
      .get(`https://localhost:7214/api/Employee/${id}`)
      .then((res) => {
        setEditData(res.data);
        setEditId(id);
      })
      .catch((error) => {
        console.log("Error:", error);
      });
  };
  const handleUpdate = (id) => {
    axios
      .put(`https://localhost:7214/api/Employee?id=${id}`, {
        id: editId,
        name: uname,
        address: uaddress,
        designation: udesignation,
        salary: usalary,
      })
      .then((response) => {
        console.log("Response:", response.data);
        setData([...data, response.data]);
        setName("");
        setAddress("");
        setDesignation("");
        setSalary("");
        window.location.reload();
        setEditId(-1);
      })
      .catch((error) => {
        console.log("Error:", error);
      });
  };

  const handleDelete = (id) => {
    console.log(`Sending DELETE request for ID: ${id}`);
    axios
      .delete(`https://localhost:7214/api/Employee?id=${id}`)
      .then((response) => {
        console.log("Delete successful:", response.data);
        setData(data.filter((user) => user.id !== id));
      })
      .catch((error) => {
        console.error("There was an error deleting the employee!", error);
      });
  };

  return (
    <>
      <Navbar.Brand className="navbar">
        <a className="nav-anchor" href="/">
          <b className="bold">iCrud</b>
        </a>
      </Navbar.Brand>
      <Container className="container">
        <Add
          className="add-section"
          handleSubmit={handleSubmit}
          setName={setName}
          setAddress={setAddress}
          setDesignation={setDesignation}
          setSalary={setSalary}
        />
        <List data={data} handleDelete={handleDelete} handleEdit={handleEdit} />
        <Edit
          handleUpdate={handleUpdate}
          useName={useName}
          useAddress={useAddress}
          useDesignation={useDesignation}
          useSalary={useSalary}
          setEditId={setEditId}
        />{" "}
        /
      </Container>
    </>
  );
};

export default App;

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

  const [editData, setEditData] = useState({});
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
        // console.log(id);
        setData(res.data);
        setEditId(id);
        // console.log(setEditData(res.data));
        // console.log(setEditId(id));
      })
      .catch((error) => {
        console.log("Error:", error);
      });
  };
  const handleUpdate = (id) => {
    axios
      .put(`https://localhost:7214/api/Employee/${id}`, {
        id: editId,
        name: editData.name,
        address: editData.address,
        designation: editData.designation,
        salary: editData.salary,
      })
      .then((response) => {
        const updatedData = data.map((user) =>
          user.id === id ? response.data : user
        );
        setData(updatedData);
        setEditData({});
        setEditId(null);
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
          setData={setData}
          handleUpdate={handleUpdate}
          editData={editData}
          setEditData={setEditData}
          editId={editId}
        />{" "}
        /
      </Container>
    </>
  );
};

export default App;

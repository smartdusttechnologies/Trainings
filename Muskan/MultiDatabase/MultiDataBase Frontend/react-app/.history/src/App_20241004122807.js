import React, { useEffect, useState } from "react";
// import "./App.css";
import { Container, Navbar } from "react-bootstrap";
import Add from "./components/Add";
// import List from "./components/List";
// import Edit from "./components/Delete";
// import Delete from "./components/Edit";
import axios from "axios";

const App = () => {
  // Define state for data, name, address, and editId
  const [data, setData] = useState([]); // To store fetched data
  const [name, setName] = useState("");
  const [address, setAddress] = useState("");
  const [designation, setDesignation] = useState("");
  const [salary, setSalary] = useState("");
  console.log(name, address);

  useEffect(() => {
    axios
      .get("https://localhost:7214/api/Employee")
      .then((res) => setData(res.data))
      .catch((error) => console.error("Error fetching data:", error));
  }, []);
  const handleSubmit = (event) => {
    event.preventDefault();
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
      })
      .catch((error) => {
        console.log("Error:", error);
      });
  };

  // const handleEdit = (id) => {
  //   setEditId(id);
  // };
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
        {/* <List data={data} />
        <Edit />
        <Delete /> */}
      </Container>
    </>
  );
};

export default App;

import React, { useEffect, useState } from "react";
// import "./App.css";
import { Container, Navbar } from "react-bootstrap";
import Add from "./components/Add";
import List from "./components/List";
// import Edit from "./components/Delete";
// import Delete from "./components/Edit";
import axios from "axios";

const App = () => {
  const [data, setData] = useState([]);
  const [name, setName] = useState("");
  const [address, setAddress] = useState("");
  const [designation, setDesignation] = useState("");
  const [salary, setSalary] = useState("");
  const [editId, setEditId] = useState(null);

  useEffect(() => {
    axios
      .get("https://localhost:7214/api/Employee")
      .then((res) => setData(res.data))
      .catch((error) => console.error("Error fetching data:", error));
  }, []);
  const handleSubmit = (event) => {
    event.preventDefault();
    const id = data.length + 1;
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
  const handelEdit = (id) => {
    axios
      .get("https://localhost:7214/api/Employee" + id)
      .then((res) => {
        useName(res.data[0].name);
        useAddress(res.data[0].address);
      })
      .catch((error) => {
        console.log("Error:", error);
      });
    setEditId(id);
  };
  const handleUpdate = (id) => {
    axios
      .put("https://localhost:7214/api/Employee" + editId, {
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
          useName={useName}
          useAddress={useAddress}
          useDesignation={useDesignation}
          useSalary={useSalary}
          setEditId={setEditId}
        />
        <List data={data} handelEdit={handelEdit} />
        {/* <Edit />
        <Delete /> */}
      </Container>
    </>
  );
};

export default App;

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
  const handleEdit = (id) => {
    const employee = data.find((user) => user.id === id);
    if (employee) {
      setEditId(id);
      setName(employee.name);
      setAddress(employee.address);
      setDesignation(employee.designation);
      setSalary(employee.salary);
    }
  };
  const handleUpdate = (event) => {
    event.preventDefault();
    axios
      .put(`https://localhost:7214/api/Employee/${editId}`, {
        name,
        address,
        designation,
        salary,
      })
      .then((response) => {
        setData(
          data.map((user) => (user.id === editId ? response.data : user))
        );

        setEditId(null);
      })
      .catch((error) => {
        console.log("Error:", error);
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

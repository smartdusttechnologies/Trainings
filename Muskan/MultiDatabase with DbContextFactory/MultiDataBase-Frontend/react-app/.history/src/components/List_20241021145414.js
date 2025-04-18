import React, { useEffect, useState } from "react";
import { Container, Navbar } from "react-bootstrap";
import Add from "./components/Add";
import List from "./components/List";
import Edit from "./components/Edit";
import axios from "axios";

const App = () => {
  const [data, setData] = useState([]);
  const [name, setName] = useState("");
  const [address, setAddress] = useState("");
  const [designation, setDesignation] = useState("");
  const [salary, setSalary] = useState("");
  const [file, setFile] = useState(null);

  const [editData, setEditData] = useState({});
  const [editId, setEditId] = useState(null);
  const [showEdit, setShowEdit] = useState(false);
  const [showAdd, setShowAdd] = useState(true);

  useEffect(() => {
    axios
      .get("http://localhost:7122/api/Employee") // Ensure this API is reachable
      .then((res) => setData(res.data))
      .catch((error) => console.error("Error fetching data:", error));
  }, []);

  const handleSubmit = (event) => {
    event.preventDefault();

    const formData = new FormData();
    formData.append("name", name);
    formData.append("address", address);
    formData.append("designation", designation);
    formData.append("salary", salary);
    if (file) formData.append("file", file);

    axios
      .post("http://localhost:7122/api/Employee", formData)
      .then((response) => {
        setData([...data, response.data]);
        setName("");
        setAddress("");
        setDesignation("");
        setSalary("");
        setFile(null); // Reset file input
      })
      .catch((error) => {
        console.error("Error:", error);
      });
  };

  const handleEdit = (id) => {
    axios
      .get(`http://localhost:7122/api/Employee/${id}`)
      .then((res) => {
        setEditData(res.data);
        setEditId(id);
        setShowEdit(true);
        setShowAdd(false);
      })
      .catch((error) => {
        console.error("Error:", error);
      });
  };

  const handleUpdate = (id) => {
    const formData = new FormData();
    formData.append("id", editId);
    formData.append("name", editData.name);
    formData.append("address", editData.address);
    formData.append("designation", editData.designation);
    formData.append("salary", editData.salary);
    if (editData.file) formData.append("file", editData.file);

    axios
      .put(`http://localhost:7122/api/Employee/${id}`, formData)
      .then((response) => {
        const updatedData = data.map((user) =>
          user.id === id ? response.data : user
        );
        setData(updatedData);
        setEditData({});
        setEditId(null);
        setShowEdit(false);
        setShowAdd(true);
      })
      .catch((error) => {
        console.error("Error:", error);
      });
  };

  const handleDelete = (id) => {
    if (window.confirm(`Are you sure you want to delete ID: ${id}?`)) {
      axios
        .delete(`http://localhost:7122/api/Employee/${id}`)
        .then((response) => {
          setData(data.filter((user) => user.id !== id));
        })
        .catch((error) => {
          console.error("Error deleting the employee:", error);
        });
    }
  };

  return (
    <>
      <Navbar.Brand className="navbar">
        <a className="nav-anchor" href="/">
          <b className="bold">iCrud</b>
        </a>
      </Navbar.Brand>
      <Container className="container">
        {showAdd && (
          <Add
            className="add-section"
            handleSubmit={handleSubmit}
            setName={setName}
            setAddress={setAddress}
            setDesignation={setDesignation}
            setSalary={setSalary}
            setFile={setFile}
          />
        )}
        {showEdit && (
          <Edit
            handleUpdate={handleUpdate}
            editData={editData}
            setEditData={setEditData}
          />
        )}
      </Container>
      <List
        className="list"
        data={data}
        handleDelete={handleDelete}
        handleEdit={handleEdit}
      />
    </>
  );
};

export default App;

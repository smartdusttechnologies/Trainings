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

  const fetchData = () => {
    axios
      .get("https://localhost:7122/api/Employee")
      .then((res) => setData(res.data))
      .catch((error) => console.error("Error fetching data:", error));
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleSubmit = (event) => {
    event.preventDefault();
    const formData = new FormData();
    formData.append("name", name);
    formData.append("address", address);
    formData.append("designation", designation);
    formData.append("salary", salary);
    if (file) {
      formData.append("file", file);
    }

    // Log formData entries
    console.log("Form Data:", Array.from(formData.entries()));

    axios
      .post("https://localhost:7122/api/Employee", formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((response) => {
        console.log("Response:", response.data);
        setData((prevData) => [...prevData, response.data]);
        setName("");
        setAddress("");
        setDesignation("");
        setSalary("");
        setFile(null);
      })
      .catch((error) => {
        console.log(
          "Error:",
          error.response ? error.response.data : error.message
        );
      });
  };

  const handleUpdate = (id) => {
    const formData = new FormData();
    formData.append("id", editId);
    formData.append("name", editData.name);
    formData.append("address", editData.address);
    formData.append("designation", editData.designation);
    formData.append("salary", editData.salary);

    axios
      .put(`https://localhost:7122/api/Employee?id=${id}`, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
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
    alert(`Sending DELETE request for ID: ${id}`);
    axios
      .delete(`https://localhost:7122/api/Employee?id=${id}`)
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
            setData={setData}
            handleUpdate={handleUpdate}
            editData={editData}
            setEditData={setEditData}
            editId={editId}
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

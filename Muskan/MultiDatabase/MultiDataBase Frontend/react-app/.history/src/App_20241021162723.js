import React, { useEffect, useState } from "react";
import { Container, Navbar } from "react-bootstrap";
import Add from "./components/Add";
import List from "./components/List";
import Edit from "./components/Edit";
import axios from "axios";

const App = () => {
  const [errorMessage, setErrorMessage] = useState(null);
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

  // Fetch all employees
  useEffect(() => {
    axios
      .get("https://localhost:7122/api/Employee")
      .then((res) => setData(res.data))
      .catch((error) => console.error("Error fetching data:", error));
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append("name", name);
    formData.append("address", address);
    formData.append("designation", designation);
    formData.append("salary", salary);
    formData.append("file", file);

    try {
      const response = await axios.post(
        "http://localhost:7122/api/Employee",
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );
      console.log("Response:", response.data);
      // Re-fetch data
      const result = await axios.get("http://localhost:7122/api/Employee");
      setData(result.data);
    } catch (error) {
      console.error("Error during fetch:", error);
    }
  };

  // Edit employee
  const handleEdit = (id) => {
    axios
      .get(`https://localhost:7122/api/Employee/${id}`)
      .then((res) => {
        setEditData(res.data);
        setEditId(id);
        setShowEdit(true);
        setShowAdd(false);
      })
      .catch((error) => console.log("Error:", error));
  };

  // Update employee
  const handleUpdate = (id) => {
    axios
      .put(`https://localhost:7122/api/Employee?id=${id}`, {
        id: editId,
        name: editData.name,
        address: editData.address,
        designation: editData.designation,
        salary: editData.salary,
        file: editData.file,
      })
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
      .catch((error) => console.log("Error:", error));
  };

  // Delete employee
  const handleDelete = (id) => {
    axios
      .delete(`https://localhost:7122/api/Employee?id=${id}`)
      .then((response) => {
        setData(data.filter((user) => user.id !== id));
      })
      .catch((error) => console.error("Error deleting employee!", error));
  };

  return (
    <>
      <Navbar.Brand className="navbar">
        <a className="nav-anchor" href="/">
          <b className="bold">iCrud</b>
        </a>
      </Navbar.Brand>
      {errorMessage && <div className="alert alert-danger">{errorMessage}</div>}
      <Container className="container">
        {showAdd && (
          <Add
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
      <List data={data} handleDelete={handleDelete} handleEdit={handleEdit} />
    </>
  );
};

export default App;

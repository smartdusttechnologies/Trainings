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

  useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await axios.get("https://localhost:7122/api/Employee");
        setData(res.data);
      } catch (error) {
        if (error.response) {
          console.error("Error data:", error.response.data);
          console.error("Error status:", error.response.status);
          setErrorMessage(
            `Error: ${error.response.status} - ${error.response.data}`
          );
        } else if (error.request) {
          console.error("Error request:", error.request);
          setErrorMessage("Network Error: No response received from server.");
        } else {
          console.error("Error message:", error.message);
          setErrorMessage("Error: " + error.message);
        }
      }
    };

    fetchData();
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
        "https://localhost:7122/api/Employee",
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );
      console.log("Response:", response.data);

      // Reset form fields
      setName("");
      setAddress("");
      setDesignation("");
      setSalary("");
      setFile(null);

      // Re-fetch data
      const result = await axios.get("https://localhost:7122/api/Employee");
      setData(result.data);
    } catch (error) {
      // Handle error message more specifically
      if (error.response) {
        setErrorMessage(
          `Error: ${error.response.status} - ${error.response.data}`
        );
      } else {
        setErrorMessage("Error: " + error.message);
      }
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
  const handleUpdate = async (id) => {
    try {
      const response = await axios.put(
        `https://localhost:7122/api/Employee/${id}`,
        {
          name: editData.name,
          address: editData.address,
          designation: editData.designation,
          salary: editData.salary,
          // If you need to send a file, use FormData (commented out here)
          file: editData.file,
        }
      );

      const updatedData = data.map((user) =>
        user.id === id ? response.data : user
      );

      setData(updatedData);
      setEditData({});
      setEditId(null);
      setShowEdit(false);
      setShowAdd(true);
    } catch (error) {
      console.error("Error during update:", error); // Log the entire error object

      // Set an error message based on the error response
      if (error.response) {
        setErrorMessage(
          `Error: ${error.response.status} - ${error.response.data}`
        );
      } else if (error.request) {
        setErrorMessage("Network Error: No response received from server.");
      } else {
        setErrorMessage("Error: " + error.message);
      }
    }
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

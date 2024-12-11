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

  const [homeAddress, setHomeAddress] = useState("");
  const [designation, setDesignation] = useState("");
  const [EmployeeSurname, setEmployeeSurname] = useState("");
  const [file, setFile] = useState(null);

  const [editData, setEditData] = useState({});
  const [editId, setEditId] = useState(null);
  const [showEdit, setshowEdit] = useState(false);
  const [showAdd, setshowAdd] = useState(true);

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
    formData.append("omeAddress", homeAddress);
    formData.append("designation", designation);
    formData.append("EmployeeSurname", EmployeeSurname);
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

      setName("");
      setHomeAddress("");
      setDesignation("");
      setEmployeeSurname("");
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
        // console.log(setEditData(res.data));
        // console.log(setEditId(id));
        setshowEdit(!showEdit);
        setshowAdd(!showAdd);
      })
      .catch((error) => console.log("Error:", error));
  };

  // Update employee
  const handleUpdate = async (id) => {
    try {
      const formData = new FormData();
      formData.append("name", editData.name);
      formData.append("homeAddress", editData.homeAddress);
      formData.append("designation", editData.designation);
      formData.append("EmployeeSurname", editData.EmployeeSurname);
      if (editData.file) {
        formData.append("file", editData.file);
      }

      console.log("Updating employee with ID:", id);
      console.log("Form Data:", {
        name: editData.name,
        homeAddress: editData.homeAddress,
        designation: editData.designation,
        EmployeeSurname: editData.EmployeeSurname,
        file: editData.file,
      });

      const response = await axios.put(
        `https://localhost:7122/api/Employee/${id}`,
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );

      const updatedData = data.map((user) =>
        user.id === id ? response.data : user
      );
      setData(updatedData);
      setEditData({});
      setEditId(null);
      setshowEdit(!showEdit);
      setshowAdd(!showAdd);
    } catch (error) {
      console.error("Error during update:", error);
      if (error.response) {
        console.error("Error data:", error.response.data);
        setErrorMessage(
          `Error: ${error.response.status} - ${JSON.stringify(
            error.response.data
          )}`
        );
      } else if (error.request) {
        setErrorMessage("Network Error: No response received from server.");
      } else {
        setErrorMessage("Error: " + error.message);
      }
    }
  };

  // Delete employee
  const handleDelete = async (id) => {
    try {
      await axios.delete(`https://localhost:7122/api/Employee/${id}`);
      alert(`Do you want to delete ${id} `);
      setData(data.filter((user) => user.id !== id));
    } catch (error) {
      console.error("Error deleting employee!", error);
      if (error.response) {
        console.error("Error data:", error.response.data);
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
            setHomeAddress={setHomeAddress}
            setDesignation={setDesignation}
            setEmployeeSurname={setEmployeeSurname}
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

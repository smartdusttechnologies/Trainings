import React, { useEffect, useState } from "react";
import { Container, Navbar } from "react-bootstrap";
import Add from "./components/Add";
import List from "./components/List";
import Edit from "./components/Edit";
import axios from "axios";

const App = () => {
  const [data, setData] = useState([]); // State to hold employee data
  const [formData, setFormData] = useState({
    // Consolidated state for adding new employee
    name: "",
    address: "",
    designation: "",
    salary: "",
  });

  const [editData, setEditData] = useState({
    // Consolidated state for editing an employee
    name: "",
    address: "",
    designation: "",
    salary: "",
  });
  const [editId, setEditId] = useState(null); // State to hold the ID of the employee being edited

  useEffect(() => {
    axios
      .get("https://localhost:7214/api/Employee") // Fetch initial data from API
      .then((res) => setData(res.data))
      .catch((error) => console.error("Error fetching data:", error));
  }, []);

  const handleSubmit = (event) => {
    event.preventDefault(); // Prevent default form submission
    axios
      .post("https://localhost:7214/api/Employee", formData) // POST request to add a new employee
      .then((response) => {
        setData([...data, response.data]); // Update the data state
        setFormData({ name: "", address: "", designation: "", salary: "" }); // Reset form fields
      })
      .catch((error) => {
        console.log("Error:", error); // Error handling
      });
  };

  const handleEdit = (id) => {
    axios
      .get(`https://localhost:7214/api/Employee/${id}`) // Fetch the employee data to edit
      .then((res) => {
        setEditData(res.data); // Set fetched data to editData state
        setEditId(id); // Store the ID of the employee being edited
      })
      .catch((error) => {
        console.log("Error:", error); // Error handling
      });
  };

  const handleUpdate = (id) => {
    axios
      .put(`https://localhost:7214/api/Employee/${id}`, editData) // PUT request to update employee data
      .then((response) => {
        const updatedData = data.map((item) =>
          item.id === id ? response.data : item
        ); // Update the local state with the updated employee data
        setData(updatedData); // Update data state
        setEditData({ name: "", address: "", designation: "", salary: "" }); // Reset edit fields
        setEditId(null); // Clear the edit ID
      })
      .catch((error) => {
        console.log("Error:", error); // Error handling
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
          formData={formData} // Pass formData to Add component
          setFormData={setFormData} // Pass setFormData function to update form fields
        />
        {/* <List data={data} handleEdit={handleEdit} /> 
        <Edit
          handleUpdate={handleUpdate}
          editData={editData} // Pass editData to Edit component
          setEditData={setEditData} // Pass setEditData function to update edit fields
          editId={editId} // Pass editId to Edit component
        /> */}
      </Container>
    </>
  );
};

export default App;

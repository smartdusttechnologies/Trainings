import React, { useEffect } from "react";
// import "./App.css";
import { Container, Navbar } from "react-bootstrap";
import Add from "./components/Add";
import List from "./components/List";
import Edit from "./components/Delete";
import Delete from "./components/Edit";
import axios from "axios";

const App = () => {
  useEffect(() => {
    axios
      .get("https://localhost:7214/api/Employee")
      .then((response) => response.json())
      .then((data) => setState(data))
      .catch((error) => console.error("Error fetching data:", error));
  }, []);
  return (
    <>
      <Navbar.Brand className="navbar">
        <a className="nav-anchor" href="/">
          <b className="bold">iCrud</b>
        </a>
      </Navbar.Brand>
      <Container className="container">
        <Add className="add-section" />
        <List />
        <Edit />
        <Delete />
      </Container>
    </>
  );
};

export default App;

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
      .get("http://localhost:5000/api/items")
      .then((response) => {
        localStorage.setItem("items", JSON.stringify(response.data));
      })
      .catch((error) => {
        console.error(error);
      });
  });
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

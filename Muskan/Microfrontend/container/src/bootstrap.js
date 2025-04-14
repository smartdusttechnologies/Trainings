import React from "react";
// import { ThemeProvider, createTheme  } from '@mui/material/styles';
import { createRoot } from "react-dom/client";

import App from "./App";
// const theme = createTheme();
// Mount function to start up the app
const mount = (el) => {
  const root = createRoot(el);
  root.render(<App />);
};

// If we are in development and in isolation,
// call the mount function immediately
if (process.env.NODE_ENV === "development") {
  const devRoot = document.getElementById("root");

  if (devRoot) {
    mount(devRoot);
  }
}

// Export mount function for container usage
export { mount };

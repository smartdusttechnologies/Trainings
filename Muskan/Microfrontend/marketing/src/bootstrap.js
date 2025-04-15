import React from "react";
import { createRoot } from "react-dom/client";
import { createMemoryHistory, createBrowserHistory } from "history";
import App from "./App";
let root = null; // Variable to store the root instance of React
// Mount function to start the micro-frontend app
const mount = (el, { onNavigate, defaultHistory, initialPath = "/" }) => {
  // Create a history object. Use defaultHistory if provided, otherwise create a memory history.
  const history =
    defaultHistory || createMemoryHistory({ initialEntries: [initialPath] });

  // Listen for navigation changes and pass them to the parent (if provided)
  history.listen(({ location }) => {
    if (onNavigate) {
      // Notify the parent container about navigation changes
      onNavigate(location); // Notify parent of navigation changes
      console.log("Navigating to location", location);
    }
  });

  if (!root) {
    root = createRoot(el); // Reassign root here, not redeclare it
  }

  // Render the App with the passed history
  root.render(<App history={history} />);

  return {
    onParentNavigate({ pathname: nextPathname }) {
      const { pathname } = history.location;
      // If the route has changed, update the history
      if (pathname !== nextPathname) {
        history.push(nextPathname);
      }
    },
  };
};

// If in development and isolation, mount the micro-frontend immediately
if (process.env.NODE_ENV === "development") {
  const devRoot = document.getElementById("marketplace-dev-root");

  if (devRoot) {
    mount(devRoot, {
      defaultHistory: createBrowserHistory(),
      // initialPath: "/",
    });
  }
}

// Export mount function for use in the container
export { mount };

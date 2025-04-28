import React from "react";
import { createRoot } from "react-dom/client";
import { createMemoryHistory, createBrowserHistory } from "history";
import App from "./App";

let root = null;
// Mount function to start the micro-frontend app
const mount = (
  el,
  { onSignIn, onNavigate, defaultHistory, initialPath = "/" }
) => {
  // const handleSignIn = () => {
  //   setIsSignedIn(true);
  // };

  // Pass this function down to the `App` component
  // <App history={history} onSignIn={handleSignIn} />;

  const history =
    defaultHistory || createMemoryHistory({ initialEntries: [initialPath] });

  // Listen for navigation changes and pass them to the parent (if provided)
  history.listen(({ location }) => {
    if (onNavigate) {
      onNavigate(location); // Ensure location is passed correctly
    }
  });

  if (!root) {
    root = createRoot(el); // Reassign root here, not redeclare it
  }

  // Render the App with the passed history
  root.render(<App history={history} onSignIn={onSignIn} />);

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
  const devRoot = document.getElementById("auth-dev-root");

  if (devRoot) {
    mount(devRoot, {
      defaultHistory: createBrowserHistory(),
      // initialPath: "/",
    });
  }
}

// Export mount function for use in the container
export { mount };

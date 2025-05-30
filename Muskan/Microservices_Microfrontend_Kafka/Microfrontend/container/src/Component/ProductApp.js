import { mount } from "product/ProductApp";
import React, { useRef, useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";

export default function AuthApp({ onSignIn }) {
  const ref = useRef(null);
  const location = useLocation();
  console.log("Current Location Path:", location.pathname);
  const navigate = useNavigate();
  console.log("navigate", navigate);

  useEffect(() => {
    const { onParentNavigate } = mount(ref.current, {
      initialPath: location.pathname,
      onNavigate: ({ pathname: nextPathname }) => {
        console.log("nextPathname", nextPathname);
        if (location.pathname !== nextPathname) {
          console.log("Next Pathaname", nextPathname);
          navigate(nextPathname);
        }
      },
      onSignIn,
      // initialPath: location.pathname,
    });

    // React Router DOM 6 handles history internally, no need for manual listeners
  }, [location, navigate]);

  return <div ref={ref} />;
}

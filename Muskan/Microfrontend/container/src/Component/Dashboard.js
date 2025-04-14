import { mount } from "dashboard/DashboardApp";
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
          console.log("location.pathname", location.pathname);
          console.log("Navigating to the next path:", nextPathname);
          navigate(nextPathname);
        }
      },
      onSignIn,
      // initialPath: location.pathname,
    });

    // React Router DOM 6 handles history internally, no need for manual listeners
    return () => {
      console.log("Unmounting MarketingApp");
    };
  }, [location, navigate]);

  return <div ref={ref} />;
}

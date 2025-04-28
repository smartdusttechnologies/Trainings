import { mount } from "marketing/MarketingApp";
import React, { useRef, useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";

export default function MarketingApp() {
  const ref = useRef(null);
  const location = useLocation();
  const navigate = useNavigate();

  useEffect(() => {
    const { onParentNavigate } = mount(ref.current, {
      initialPath: location.pathname,
      onNavigate: ({ pathname: nextPathname }) => {
        if (location.pathname !== nextPathname) {
          console.log("Next Pathaname", nextPathname);
          navigate(nextPathname); // Navigate inside container
        }
      },
    });
    // Syncing route changes FROM container TO remote
    const unlisten = () => {
      // this effect re-runs when location changes
      onParentNavigate({ pathname: location.pathname });
    };

    unlisten(); // call once on mount
  }, [location, navigate]);

  return <div ref={ref} />;
}

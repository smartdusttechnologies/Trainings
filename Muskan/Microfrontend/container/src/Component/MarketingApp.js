// import { mount } from "marketing/MarketingApp";
// import React, { useRef, useEffect } from "react";
// import { useLocation, useNavigate } from "react-router-dom";

// export default function MarketingApp({ currentPathname }) {
//   const ref = useRef(null);
//   const navigate = useNavigate();

//   useEffect(() => {
//     const { onParentNavigate } = mount(ref.current, {
//       initialPath: currentPathname,
//       onNavigate: ({ pathname: nextPathname }) => {
//         if (currentPathname !== nextPathname) {
//           navigate(nextPathname);
//         }
//       },
//     });

//     return () => {
//       console.log("Unmounting MarketingApp");
//     };
//   }, [currentPathname, navigate]);

//   return <div ref={ref} />;
// }

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
          navigate(nextPathname); // Navigate inside container
        }
      },
    });

    const unlisten = () => {
      const observer = new MutationObserver(() => {
        onParentNavigate({ pathname: location.pathname }); // Notify MFE if container changes
      });

      observer.observe(ref.current, { childList: true, subtree: true });
      return () => observer.disconnect();
    };

    const cleanup = unlisten();
    return cleanup;
  }, [location, navigate]);

  return <div ref={ref} />;
}

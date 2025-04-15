import { Auth0Provider } from "@auth0/auth0-react";
import React from "react";

export function AuthProvider({ children }) {
  //   console.log("React context:", React);
  //   console.log("Auth0Provider:", children);
  //   console.log("Auth0Provider props:", Auth0Provider.defaultProps);

  const domain = "dev-h2hafjnbquckxeji.us.auth0.com";
  const clientId = "PpKDMLdEwNYjHRMxYuUMtsjyVger2eAu";

  return (
    <Auth0Provider
      domain={domain}
      clientId={clientId}
      authorizationParams={{
        redirect_uri: `${window.location.origin}/callback`,
        audience: "https://localhost:6064",
        scope: "openid profile email read:basket",
      }}
    >
      {children}
    </Auth0Provider>
  );
}

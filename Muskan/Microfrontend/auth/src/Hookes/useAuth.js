import { useContext } from "react";
import { Auth0Context } from "@auth0/auth0-react";

export const useAuth = () => {
  const context = useContext(Auth0Context);
  if (!context) {
    throw new Error("useAuth must be used within an Auth0Provider");
  }
  return context;
};

import { useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";

export default function Callback() {
  const { search } = useLocation();
  const navigate = useNavigate();

  useEffect(() => {
    const params = new URLSearchParams(search);
    const token = params.get("token");

    if (token) {
      localStorage.setItem("auth_token", token);
      navigate("/dashboard");
    } else {
      navigate("/auth/login");
    }
  }, [search]);

  return <div>Processing login...</div>;
}

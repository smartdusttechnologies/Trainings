import { createContext, useState } from "react";
import { Navigate } from "react-router-dom";

const AuthContext = createContext({});

export const AuthProvider = ({children}) => {
    const [auth , setAuth] = useState({})

    return (
        <AuthContext.Provider value={{auth , setAuth}}>
            {children}
        </AuthContext.Provider>
    )
}

export default AuthContext
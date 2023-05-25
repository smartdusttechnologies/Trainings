import React, { useContext } from 'react'
import AuthContext from './AuthProvider'
import axios from 'axios'

const AuthTokenProvider = () => {
    const {auth} = useContext(AuthContext)
    console.log(auth)
    console.log(auth.accessToken)
    const api = 'https://localhost:7023/Security'

    const response =  axios.get(api, { headers: {"Authorization" : `${auth.accessToken}`} })
        .then(res => {
            console.log(res.data);
        // this.setState({
        //     items: res.data,  /*set response data in items array*/
        //     isLoaded : true,
        //     redirectToReferrer: false
        // })
        })
        console.log(response)
  return (response)
}

export default AuthTokenProvider

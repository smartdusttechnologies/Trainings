import React, { useContext, useState } from 'react'
import './Login.css'
import { Link, Navigate } from 'react-router-dom'
import axios from 'axios';
import AuthContext from '../../context/AuthProvider';
import { TextField } from '@mui/material';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const loginurl = 'https://localhost:7023/Security/Login';

const Login = () => {
  const {auth , setAuth} = useContext(AuthContext)

  const [email , setEmail] = useState('');
  const [password , setPassword] = useState('');
  const [msg , setMsg] = useState('')

  
  const handleSubmit = async (e)=>{
    e.preventDefault()

    try {
      const response = await axios.post(loginurl , 
      {
        userName:email,
        password:password
      }        
      )
      .then(response=> {
        console.log(response?.data)
        // console.log(response?.data.validationMessages[0].reason)
        const accessToken = response?.data.requestedObject.accessToken 
        const userName = response?.data.requestedObject.userName
        const userId = response?.data.requestedObject.userId
        const isAuthenticated = response?.data.isSuccessful
        // const errmsg = response?.data.validationMessages[0].reason
        console.log(accessToken,userName,userId)
        
        setAuth({accessToken , userName , userId , isAuthenticated})
        // setMsg(errmsg)

          toast.success("Sign in Successful!",{
            position: "bottom-center",
            autoClose: 5000,
            hideProgressBar: true,
            closeOnClick: true,
            pauseOnHover: false,
            draggable: true,
            progress: undefined,
            theme: "colored",
          });
        
      })
      .catch(err=>{
        // console.log(err)
        toast.error("UserName or password mismatch.",{
          position: "bottom-center",
          autoClose: 5000,
          hideProgressBar: true,
          closeOnClick: true,
          pauseOnHover: false,
          draggable: true,
          progress: undefined,
          theme: "colored",
          });
      })
      
    } catch (err) {
      console.log(err);
    }
  }


  return (
    <div className='login-page'>
      <div className='login-container'>
        <div className='login-header'>
          <div className='login-text-div'>
            <Link style={{textDecoration:'none' , color:"blue"}}>
            <p className='text-login'>Sign in</p>
            </Link>
          </div>
          <div>
            <Link to={'/signup'} style={{textDecoration:'none'}}>
            <p style={{fontSize:"23px", color: 'rgb(62, 61, 61)'}}>Sign up</p>
            </Link>
          </div>
        </div>
        <form onSubmit={(e)=>handleSubmit(e)} action="">
          <TextField size='small'
           onChange={(e)=>setEmail(e.target.value)}
          label='Username or Email ID' type="text"
          value={email}
          />
          <TextField size='small'
           onChange={(e)=>setPassword(e.target.value)}
          label='Password' type="password"
          value={password}
          />
          <div className='remeber-me-forgot-pass'>
           <div> <label htmlFor="">Remember me</label> <input type="checkbox" /></div>
          <Link to={'/forgotpassword'} className='forgot-pass'>Forgot password</Link>
          </div>
          <input className='submit-btn' type="submit" value={'Sign-in'}/>
        </form>
        <div>
          {msg}
        </div>
        <div className='Or-div'>
          <div><hr /></div>
          <div><p>or</p></div>
          <div><hr /></div>
        </div>

        <div className='social-media-login'>
          <a className='Twitter blue-login' href=""><p>Twitter</p></a>
          <a className='Facebook blue-login' href=""><p>Facebook</p></a>
          <a className='Google red-login' href=""><p>Google</p></a>
          <a className='Linked-In blue-login' href=""><p>Linked-In</p></a>
        </div>
        <div className='sign-up'>
          <Link to={'/signup'}>Don't have an account? Sign Up</Link>
        </div>
        <div className='why-box'>
          <p class="why">Why Create an Account?</p>
          <div>
              <p class="ans">
                  By creating the account you agreed to our
                  <a>Privacy Policy</a>&
                  <a>Cookie Policy</a>
              </p>
          </div>
        </div>
      </div>
      <ToastContainer/>
    </div>
  )
}

export default Login
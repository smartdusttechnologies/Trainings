import React, { useContext, useState } from 'react'
import { Link } from 'react-router-dom'
import './Signup.css'
import axios from 'axios'
import { TextField } from '@mui/material'
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import AuthContext from '../../context/AuthProvider'

const signupapi = 'https://localhost:7023/Security/SignUp';

const Signup = () => {
  const {auth , setAuth , notification , setNotification} = useContext(AuthContext)


  const [newuser , setNewuser] = useState({
    firstname:"",
    lastname:"",
    username:"",
    mail:"",
    phone:0,
    country:"",
    org:"",
    password:"",
    confirmpassword:""
  })
  const [msg , setMsg] = useState('');


  const handleChange = (e)=>{
    const newdata = {...newuser}
    newdata[e.target.id] = e.target.value
    setNewuser(newdata)
    console.log(newuser)
  }

  const handleSubmit = (e)=>{
    e.preventDefault()
    setMsg('')
      axios.post( signupapi , {
        id:0,
        userName: newuser.username,
        firstName: newuser.firstname,
        lastName: newuser.lastname,
        email:newuser.mail,
        mobile:newuser.phone,
        country:newuser.country,
        isdCode:'',
        mobileValidationStatus: 0,
        orgId:newuser.org,
        password:newuser.password,
        newPassword:newuser.confirmpassword
        })
        .then(res=> {
          console.log(res)
          setMsg(res?.data.message)

          toast.success(`${msg}`,{
            position: "bottom-center",
            autoClose: 5000,
            hideProgressBar: true,
            closeOnClick: true,
            pauseOnHover: false,
            draggable: true,
            progress: undefined,
            theme: "colored",
            });

            setNotification([...notification,"Sign up Successful!"])
        })
        .catch(err=>{
          console.log(err)
          toast.error("Enter right credentials.",{
            position: "bottom-center",
            autoClose: 5000,
            hideProgressBar: true,
            closeOnClick: true,
            pauseOnHover: false,
            draggable: true,
            progress: undefined,
            theme: "colored",
            });
            setNotification([...notification,"Sign up Unsuccessful Enter right credentials.!"])

        })
  }
  return (
    <div className='signup-page'>
      <div className='signup-container'>
        <div className='login-header'>
          <div className='login-text-div'>
            <Link  to={'/login'}  style={{textDecoration:'none'}}>
            <p style={{fontSize:"23px", color: 'rgb(62, 61, 61)'}}>Sign in</p>
            </Link>
          </div>
          <div>
            <Link style={{textDecoration:'none'}}>
            <p className='text-signup'>Sign up</p>
            </Link>
          </div>
        </div>
        <form onSubmit={(e)=>handleSubmit(e)} action="">
          <TextField size='small' onChange={(e)=>handleChange(e)} id='firstname' label='Enter FirstName' type="text"/>
          <TextField size='small' onChange={(e)=>handleChange(e)} id='lastname' label='Enter LastName' type="text"/>
          <TextField size='small' onChange={(e)=>handleChange(e)} id='username' label='Enter UserName' type="text"/>
          <TextField size='small' onChange={(e)=>handleChange(e)} id='mail' label='Enter Email' type="text"/>
          <TextField size='small' onChange={(e)=>handleChange(e)} id='phone' label='Enter MobileNumber' type="number"/>

          <select  onChange={(e)=>handleChange(e)} id="country">
            <option value="">Country</option>
            <option value="india">India</option>
          </select>

          <select onChange={(e)=>handleChange(e)} id='org'>
            <option value="sysorg">SYSORG</option>
            <option value={0}>ORG 1</option>
          </select>

          <TextField size='small' onChange={(e)=>handleChange(e)} id='password' label='Enter Password' type="password"/>
          <TextField size='small' onChange={(e)=>handleChange(e)} id='confirmpassword' label='Re-Enter Password' type="password"/>
          <button className='submit-btn'>Sign up</button>
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
        {/* <div className='login'>
          <Link to={'/login'}>Already have an account? Sign in</Link>
        </div> */}
      </div>
      <ToastContainer/>
    </div>
  )
}

export default Signup

import React, { useState } from 'react'
import { Link } from 'react-router-dom'
import './Signup.css'
import axios from 'axios'
const Signup = () => {

  const signupapi = 'https://localhost:7023/Security/SignUp';

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

  const handleChange = (e)=>{
    const newdata = {...newuser}
    newdata[e.target.id] = e.target.value
    setNewuser(newdata)
    console.log(newuser)
  }

  const handleSubmit = async (e)=>{
    e.preventDefault()
    try {
      axios.post( signupapi , {
        userName: newuser.username,
        firstName: newuser.firstname,
        lastName: newuser.lastname,
        email:newuser.mail,
        mobile:newuser.phone,
        country:newuser.country,
        org:newuser.org,
        password:newuser.password,
        confirmpassword:newuser.confirmpassword
        })
        .then(res=> console.log(res))
        .catch(err=>console.log(err))
    } catch (error) {
      
    }
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
          <input onChange={(e)=>handleChange(e)} id='firstname' placeholder='Enter FirstName' type="text"/>
          <input onChange={(e)=>handleChange(e)} id='lastname' placeholder='Enter LastName' type="text"/>
          <input onChange={(e)=>handleChange(e)} id='username' placeholder='Enter UserName' type="text"/>
          <input onChange={(e)=>handleChange(e)} id='mail' placeholder='Enter Email' type="text"/>
          <input onChange={(e)=>handleChange(e)} id='phone' placeholder='Enter MobileNumber' type="number"/>

          <select  onChange={(e)=>handleChange(e)} id="country">
            <option value="">Country</option>
            <option value="india">India</option>
          </select>

          <select onChange={(e)=>handleChange(e)} id='org'>
            <option value="sysorg">SYSORG</option>
            <option value={1}>ORG 1</option>
          </select>

          <input onChange={(e)=>handleChange(e)} id='password' placeholder='Enter Password' type="password"/>
          <input onChange={(e)=>handleChange(e)} id='confirmpassword' placeholder='Re-Enter Password' type="password"/>
          <button className='submit-btn'>Sign up</button>
        </form>
        
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
    </div>
  )
}

export default Signup

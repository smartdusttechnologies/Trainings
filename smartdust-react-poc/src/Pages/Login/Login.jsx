import React, { useState } from 'react'
import './Login.css'
import { Input, TextField } from '@mui/material'
import { Link } from 'react-router-dom'
const Login = () => {
  const [email , setEmail] = useState('');
  const [password , setPassword] = useState('');

  const handleSubmit = async ()=>{
    try {
      
    } catch (error) {
      
    }
  }


  return (
    <div className='login-page'>
      <div className='login-container'>
        <p className='text-login'>Sign in</p>
        <form action="">
          <TextField
           onChange={(e)=>setEmail(e.target.value)}
          label='Email Address' type="text"
          value={email}
          />
          <TextField
           onChange={(e)=>setPassword(e.target.value)}
          label='Password' type="password"
          value={password}
          />
          <Link className='forgot-pass'>Forgot password</Link>
          <input className='submit-btn' type="submit" value={'Sign-in'}/>
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
    </div>
  )
}

export default Login

import React, { useState } from 'react'
import './Login.css'
import { Input, TextField } from '@mui/material'
import { Link } from 'react-router-dom'
const Login = () => {
  const [email , setEmail] = useState('');
  const [password , setPassword] = useState('');
  const [errmsg , setErrmsg] = useState('')

  const handleSubmit = async ()=>{
    try {

      
      setEmail('')
      setPassword('')
    } catch (err) {
      
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
        <form action="">
          <input
           onChange={(e)=>setEmail(e.target.value)}
          placeholder='Username or Email ID' type="text"
          value={email}
          />
          <input
           onChange={(e)=>setPassword(e.target.value)}
          placeholder='Password' type="password"
          value={password}
          />
          <div className='remeber-me-forgot-pass'>
           <div> <label htmlFor="">Remember me</label> <input type="checkbox" /></div>
          <Link to={'/forgotpassword'} className='forgot-pass'>Forgot password</Link>
          </div>
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

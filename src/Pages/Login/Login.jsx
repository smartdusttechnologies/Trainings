import React from 'react'
import './Login.css'
import { Input, TextField } from '@mui/material'
import { Link } from 'react-router-dom'
const Login = () => {
  return (
    <div className='login-page'>
      <div className='login-container'>
        <p className='text-login'>Sign in</p>
        <form action="">
          <TextField label='Email Address' type="text"/>
          <TextField label='Password' type="password"/>
          <input className='submit-btn' type="submit" value={'Login'}/>
        </form>
        <div className='sign-up'>
          <Link>Forgot password</Link>
          <Link to={'/signup'}>Don't have an account? Sign Up</Link>
        </div>
      </div>
    </div>
  )
}

export default Login

import { TextField } from '@mui/material'
import React from 'react'
import { Link } from 'react-router-dom'
import './Signup.css'
const Signup = () => {
  return (
    <div className='signup-page'>
      <div className='signup-container'>
        <p className='text-signup'>Sign up</p>
        <form action="">
          <TextField label='Name' type="text"/>
          <TextField label='Email Address' type="text"/>
          <TextField label='Password' type="password"/>
          <input className='submit-btn' type="submit" value={'Sign up'}/>
        </form>
        <div className='login'>
          <Link to={'/login'}>Already have an account? Sign in</Link>
        </div>
      </div>
    </div>
  )
}

export default Signup

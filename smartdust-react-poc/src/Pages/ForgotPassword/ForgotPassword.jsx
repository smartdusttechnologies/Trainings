import React from 'react'
import './ForgotPass.css'
import { Button, Input, TextField } from '@mui/material'

const ForgotPassword = () => {
  return (
    <div className='Forgotpass-body'>
      <div className='forgotpass-container'>
        <div>
            <TextField label="Enter Registered Email" size='small' />
            <Button id='send-otp' variant="contained" background-color='red'>Send OTP</Button>
        </div>
        <div>
            <TextField label="OTP" size='small' type='number' />
            <Button id='confirm-otp' variant="contained" background-color='red'>Confirm</Button>
        </div>
      </div>
    </div>
  )
}

export default ForgotPassword

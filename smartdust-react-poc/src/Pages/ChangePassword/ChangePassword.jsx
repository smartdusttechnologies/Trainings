import React, { useState } from 'react'
import './ChangePassword.css'
import axios from 'axios'
import { Button, TextField } from '@mui/material';


const ChangePassword = () => {
  const api = 'https://localhost:7023/Security/ChangePassword';

  const [oldPassword , setOldpassword] = useState('')
  const [newPassword , setNewpassword] = useState('')
  const [confirmPassword , setConfirmpassword] = useState('')

  const handleSubmit = (e)=>{
    e.preventDefault()
    // console.log(e)
    console.log(oldPassword,newPassword,confirmPassword)

    axios.post(api , {
      id:5,
      oldPassword,
      newPassword,
      confirmPassword
    })
    .then(res=>{
      console.log(res)
    })
    .catch(err =>{
      console.log(err)
    })
  }


  return (
    <div className='Changepass-body'>
      <div className='Changepass-container'>
        <p>Change Password</p>
        {/* <form > */}
          <div className='changepass-inputs'>

          <TextField onChange={(e)=> setOldpassword(e.target.value)} label='OldPassword' size='small' type="password" />
          <TextField onChange={(e)=> setNewpassword(e.target.value)} label='NewPassword' size='small' type="password" />
          <TextField onChange={(e)=> setConfirmpassword(e.target.value)} label='ConfirmPassword' size='small' type="password" />
          </div>
          <div className='changepass-save'>
            <div>
              <Button onClick={(e)=>handleSubmit(e)} id='save-btn'>Save</Button>
            </div>
            <div>

              <Button id='cancel-btn'>Cancel</Button>
            </div>
          </div>
        {/* </form> */}
      </div>
    </div>
  )
}

export default ChangePassword

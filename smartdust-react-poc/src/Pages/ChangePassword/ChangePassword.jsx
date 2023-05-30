import React, { useContext, useState } from 'react'
import './ChangePassword.css'
import axios from 'axios'
import { Button, TextField } from '@mui/material';
import AuthContext from '../../context/AuthProvider';


const ChangePassword = () => {
  const api = 'https://localhost:7023/Security/ChangePassword';
  const {auth} = useContext(AuthContext)


  const [oldPassword , setOldpassword] = useState('');
  const [newPassword , setNewpassword] = useState('');
  const [confirmPassword , setConfirmpassword] = useState('');
  const [msg , setMsg] = useState('');


  const handleSubmit = (e)=>{
    e.preventDefault()
    // console.log(e)
    console.log(oldPassword,newPassword,confirmPassword,auth.userName,auth.userId)

    axios.post(api , {
      id:auth.userId,
      oldPassword,
      newPassword,
      confirmPassword,
      username: auth.userName,
      userId: auth.userId,
      passwordHash: '',
      passwordSalt: ''
    },{
       headers: {"Authorization" : `${auth.accessToken}`}
    })
    .then(res=>{
      console.log(res)
      console.log(res?.data)
      res?.data ? setMsg("Password Changed Succesfully") : setMsg('')
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
          <div>
            {msg}
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

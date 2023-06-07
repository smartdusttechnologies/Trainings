import React, { useContext, useState } from 'react'
import './ChangePassword.css'
import axios from 'axios'
import { Button, TextField } from '@mui/material';
import AuthContext from '../../context/AuthProvider';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { useNavigate } from 'react-router-dom';

const api = 'https://localhost:7023/Security/ChangePassword';

const ChangePassword = () => {
  const navigate = useNavigate()
  const {auth , setAuth , notification , setNotification} = useContext(AuthContext)


  const [oldPassword , setOldpassword] = useState('');
  const [newPassword , setNewpassword] = useState('');
  const [confirmPassword , setConfirmpassword] = useState('');
  const [msg , setMsg] = useState('');


  const handleSubmit = (e)=>{
    e.preventDefault()
    // console.log(e)
    console.log(oldPassword,newPassword,confirmPassword,auth.userName,auth.userId)

    axios.post(api , {
      oldPassword,
      newPassword,
      confirmPassword,
      userId: auth.userId,
      username: auth.userName
    },{
       headers: {"Authorization" : `${auth.accessToken}`}
    })
    .then(res=>{
      console.log(res)
      console.log(res?.data)
      
      toast.success("Password Changed Successfully!",{
        position: "bottom-center",
        autoClose: 5000,
        hideProgressBar: true,
        closeOnClick: true,
        pauseOnHover: false,
        draggable: true,
        progress: undefined,
        theme: "colored",
      });
      setNotification([...notification,"Password Changed Successfully!"])
      setNotification([...notification, {message:"Password Changed Successfully!",success:res?.data}])

      setTimeout(() => {
        navigate('/')
      }, 3500);
    })
    .catch(err =>{
      console.log(err)
      
      toast.error("Enter Right Password!",{
        position: "bottom-center",
        autoClose: 5000,
        hideProgressBar: true,
        closeOnClick: true,
        pauseOnHover: false,
        draggable: true,
        progress: undefined,
        theme: "colored",
      });
      setNotification([...notification,"Password Not Changed Enter Right Password!"])

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
          {/* <div>
            {msg}
          </div> */}
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
      <ToastContainer/>
    </div>
  )
}

export default ChangePassword

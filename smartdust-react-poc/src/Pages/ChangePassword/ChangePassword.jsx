import React, { useState } from 'react'
import './ChangePassword.css'
const ChangePassword = () => {
  const [oldpassword , setOldpassword] = useState('')
  const [newpassword , setNewpassword] = useState('')
  const [confirmpassword , setConfirmpassword] = useState('')

  console.log(oldpassword)
  return (
    <div className='Changepass-body'>
      <div className='Changepass-container'>
        <p>Change Password</p>
        <div className='changepass-inputs'>

        <input onChange={(e)=> setOldpassword(e.target.value)} placeholder='OldPassword' type="password" />
        <input onChange={(e)=> setNewpassword(e.target.value)} placeholder='NewPassword' type="password" />
        <input onChange={(e)=> setConfirmpassword(e.target.value)} placeholder='ConfirmPassword' type="password" />
        </div>
        <div className='changepass-save'>
          <div>

            <button id='save-btn'>Save</button>
          </div>
          <div>

            <button id='cancel-btn'>Cancel</button>
          </div>
        </div>
      </div>
    </div>
  )
}

export default ChangePassword

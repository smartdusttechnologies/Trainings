import React from 'react'
import './ChangePassword.css'
const ChangePassword = () => {
  return (
    <div className='Changepass-body'>
      <div className='Changepass-container'>
        <p>Change Password</p>
        <div className='changepass-inputs'>

        <input placeholder='OldPassword' type="password" />
        <input placeholder='NewPassword' type="password" />
        <input placeholder='ConfirmPassword' type="password" />
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

import React from 'react'
import './Contact.css'

const Contact = () => {
  return (
    <div className='Contact-body'>
      <div className='Contact-details'>
        <div className='smartdust-details'>
          <p id='title'>GET IN TOUCH</p>
          <p>Bhatta Road, Danapur, Patna - 801503</p>
          <p>rishirodeo@hotmail.com</p>
          <p>7857068847</p>
        </div>
        <div className='user-details'>
          <form className='user-details-form' action=""> 
          <div className='flex-input-div'>
            <div>
              <label htmlFor="">Name</label> <br />
              <input type="text" placeholder='Enter your name' />
            </div>
            <div>
              <label htmlFor="">Email</label> <br />
              <input type="email" placeholder='Enter your email' />
            </div>
          </div>
          <div className='flex-input-div'>
            <div>
              <label htmlFor="">Phone</label> <br />
              <input type="number" placeholder='Enter your phone number' />
            </div>
            <div>
              <label htmlFor="">Address</label> <br />
              <input type="text" placeholder='Enter your address' />
            </div>
          </div>
          <div className='long-input'>
            <label htmlFor="">Subject</label> <br />
            <input type="text" placeholder='Type your subject' />
          </div>
          <div className='long-input'>
            <label htmlFor="">Message</label> <br />
            <input type="text" placeholder='Type your message here' />
          </div>
          <div  className='submit-user-details'>
            <input type="submit" />
          </div>
          </form>
        </div>
      </div>
    </div>
  )
}

export default Contact

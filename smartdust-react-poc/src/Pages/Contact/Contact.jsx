import React, { useState } from 'react'
import './Contact.css'
import axios from "axios";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const Contact = () => {
  // API Link 
  const APIurl = 'https://localhost:7023/Home/Contactus';

  // User Details 
  const [userdata , setUserdata] = useState({
    name: "",
    mail:"",
    phone:0,
    address:"",
    subject:"",
    message:""
  })

  const handleChange = (e)=>{
    const newdata = {...userdata }
    newdata[e.target.id] = e.target.value
    setUserdata(newdata);
    console.log(userdata);
  }

  const handleSubmit = (e)=>{
    e.preventDefault()
    console.log(userdata)
    axios.post( APIurl , {
    name: userdata.name,
    mail:userdata.mail,
    phone:userdata.phone,
    subject:userdata.subject,
    address:userdata.address,
    message:userdata.message
    })
    .then(res=> {
      console.log(res?.data)
      
      toast.success("Submitted successfully!",{
        position: "bottom-center",
        autoClose: 5000,
        hideProgressBar: true,
        closeOnClick: true,
        pauseOnHover: false,
        draggable: true,
        progress: undefined,
        theme: "dark",
      });
    })
    .catch(err=>{
      console.log(err)
      
      toast.error("Enter right credentials!",{
        position: "bottom-center",
        autoClose: 5000,
        hideProgressBar: true,
        closeOnClick: true,
        pauseOnHover: false,
        draggable: true,
        progress: undefined,
        theme: "dark",
      });
    })
  }


  return (
    <div className='Contact-body'>
      <div className='Contact-details'>
        <div className='smartdust-details'>
          <p id='title'>GET IN TOUCH</p>
          <p>Bhatta Road, Danapur, Patna - 801503</p>
          <p>rishirodeo@hotmail.com</p>
          <p>7857068847</p>
        </div>

        {/* User Detail Form */}
        <div className='user-details'>
          <form onSubmit={(e)=>handleSubmit(e)} className='user-details-form' > 
          <div className='flex-input-div'>
            <div>
              <label htmlFor="">Name</label> <br />
              <input onChange={(e)=>handleChange(e)} required id='name' value={userdata.name} type="text" placeholder='Enter your name' />
            </div>
            <div>
              <label htmlFor="">Email</label> <br />
              <input onChange={(e)=>handleChange(e)} required id='mail' value={userdata.mail} type="email" placeholder='Enter your email' />
            </div>
          </div>
          <div className='flex-input-div'>
            <div>
              <label htmlFor="">Phone</label> <br />
              <input onChange={(e)=>handleChange(e)} required id='phone' value={userdata.phone} type="number" placeholder='Enter your phone number' />
            </div>
            <div>
              <label htmlFor="">Address</label> <br />
              <input onChange={(e)=>handleChange(e)} id='address' value={userdata.address} type="text" placeholder='Enter your address' />
            </div>
          </div>
          <div className='long-input'>
            <label htmlFor="">Subject</label> <br />
            <input onChange={(e)=>handleChange(e)} id='subject' value={userdata.subject} type="text" placeholder='Type your subject' />
          </div>
          <div className='long-input'>
            <label htmlFor="">Message</label> <br />
            <input onChange={(e)=>handleChange(e)} id='message' value={userdata.message} type="text" placeholder='Type your message here' />
          </div>
          <div  className='submit-user-details'>
            <input type="submit" />
          </div>
          </form>
        </div>
      </div>
      <ToastContainer/>
    </div>
  )
}

export default Contact

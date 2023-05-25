import React, { useContext, useEffect } from 'react'
import SlickGoTo from '../../components/AutoSlider/AutoSlider'
import AuthTokenProvider from '../../context/AuthTokenProvider'
import AuthContext from '../../context/AuthProvider'
import axios from 'axios'

const About = () => {
  const {auth} = useContext(AuthContext)
  const api = 'https://localhost:7023/Security'
  console.log(auth)
  console.log(auth.accessToken)
      const handleAuth = ()=>{

        axios.get(api, { headers: {"Authorization" : `${auth.accessToken}`} })
        .then(res => {
          console.log(res.data);
          // this.setState({
          //       items: res.data,  /*set response data in items array*/
          //       isLoaded : true,
          //       redirectToReferrer: false
          //   })
          })
      }
  
        useEffect(() => {
          handleAuth()
        })

  return (
    <div>

    {/* About Section  */}
    <div className='About'>
      <div className='About-container'>
        
        <p id='About-Q'>OUR BUSINESS</p>
        <p id='About-text'>
            With our innovative and insightful technology, we strive to enhance our users’ every day experiences. Founded in 2000, our incredible team of engineers, programmers, designers and marketers have worked tirelessly to bring Smartdust Technologies to the forefront of the industry. We will continue to work relentlessly to become the technological standard, providing big picture insights and solutions for companies of all sizes. Get in touch to learn more.</p>
      </div>
    </div>

      {/* Auto Slider  */}
      <div className='Auto-slider'>
        <SlickGoTo/>
      </div>

    </div>
  )
}

export default About

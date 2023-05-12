import React from 'react'
import SlickGoTo from '../../components/AutoSlider/AutoSlider'

const About = () => {
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

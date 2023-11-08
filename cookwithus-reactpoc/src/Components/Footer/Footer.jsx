import React from 'react'
import './Footer.css'
import { GrFacebookOption,GrLinkedinOption } from 'react-icons/gr';
import { AiOutlineTwitter } from 'react-icons/ai';
import { useTheme } from '../../context/ThemeContext';


const Footer = () => {
  const { darkMode } = useTheme();

  return (
    <div className='Footer-body' style={{backgroundColor: !darkMode ? '#489bee' : '#101418' }}>
      <div className='Footer'>
        <div className='Social-media'>
         <a href="" target='_blank'><GrFacebookOption/></a> 
         <a href="" target='_blank'><AiOutlineTwitter/></a> 
         <a href="" target='_blank'><GrLinkedinOption/></a> 
        </div>
        <div className='Footer-text'>
          <p>©2021 by Cook With Us Pvt. Ltd. Terms & Conditions / Privacy Policy</p>
        </div>
      </div>
    </div>
  )
}

export default Footer

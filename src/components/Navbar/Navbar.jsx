import React from 'react'
import './Navbar.css'
import { Link } from 'react-router-dom'
import Drawer from '../Drawer/Drawer'

const Navbar = () => {
  return (
    <div className='Navbar-body'>
      <div className='Navbar'>
        <div className='left'>
          <Link to={'/'}>
          <div>
            <img src="https://static.wixstatic.com/media/d8046d_8943b656e6a5451faea805bd3fff9196~mv2.png/v1/crop/x_11,y_0,w_2772,h_668/fill/w_274,h_66,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/logo_edited.png" alt="" />
          </div>
          </Link>
        </div>
        <div className='right none'>
        <Link to={'/'}><div><a href="">Home</a> </div></Link> |
        <Link to={'/contact'}><div><a href="">Contact</a> </div></Link> | 
        <Link to={'/about'}><div><a href="">About</a> </div></Link>
        </div>
      </div>
          <Drawer/>
    </div>
  )
}

export default Navbar
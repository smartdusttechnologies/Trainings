import React from 'react'
import './BottomNav.css'
import { BottomNavigationAction } from '@mui/material';
import Box from '@mui/material/Box';
import BottomNavigation from '@mui/material/BottomNavigation';
import Drawer from '../Drawer/Drawer';
import FacebookIcon from '@mui/icons-material/Facebook';
import EmailIcon from '@mui/icons-material/Email';
import LocalPhoneIcon from '@mui/icons-material/LocalPhone';
import ChatBubbleIcon from '@mui/icons-material/ChatBubble';

const BottomNav = () => {
  const [value, setValue] = React.useState(0);

  return (
    <div className='BottomNavigation-container'>
    <Box sx={{ width: 500 }}>
      <BottomNavigation
        showLabels
        value={value}
        onChange={(event, newValue) => {
          setValue(newValue);
        }}
      >
        <BottomNavigationAction label="Phone" icon={<LocalPhoneIcon />} />
        <BottomNavigationAction label="Email" icon={<EmailIcon />} />
        <BottomNavigationAction label="Facebiik" icon={<FacebookIcon />} />
        <BottomNavigationAction label="Chat" icon={<ChatBubbleIcon />} />
      </BottomNavigation>
    </Box>
    </div>
  )
}

export default BottomNav

import React from 'react'
import './BottomNav.css'
import { BottomNavigationAction } from '@mui/material';
import Box from '@mui/material/Box';
import BottomNavigation from '@mui/material/BottomNavigation';
import FacebookIcon from '@mui/icons-material/Facebook';
import EmailIcon from '@mui/icons-material/Email';
import LocalPhoneIcon from '@mui/icons-material/LocalPhone';
import ChatBubbleIcon from '@mui/icons-material/ChatBubble';
import Paper from '@mui/material/Paper';

const BottomNav = () => {
  const [value, setValue] = React.useState(0);

  return (
    <div className='BottomNavigation-container'>
    <Box sx={{ width: 500 }}>
      
      <Paper sx={{ position: 'fixed', bottom: 0, left: 0, right: 0 }} elevation={3}>

        <BottomNavigation
          showLabels
          value={value}
          onChange={(event, newValue) => {
            setValue(newValue);
          }}
          >
          <BottomNavigationAction label="Phone" icon={<LocalPhoneIcon />} />
          <BottomNavigationAction label="Email" icon={<EmailIcon />} />
          <BottomNavigationAction label="Facebook" icon={<FacebookIcon />} />
          <BottomNavigationAction label="Chat" icon={<ChatBubbleIcon />} />
        </BottomNavigation>
      </Paper>
    </Box>
    </div>
  )
}

export default BottomNav

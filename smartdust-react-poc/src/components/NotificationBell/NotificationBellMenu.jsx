import * as React from 'react';
import Button from '@mui/material/Button';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import NotificationsIcon from '@mui/icons-material/Notifications';
import AuthContext from '../../context/AuthProvider';

export default function NotificationBellMenu() {

  const {notification} = React.useContext(AuthContext);

    const [anchorEl, setAnchorEl] = React.useState(null);
    const open = Boolean(anchorEl);
  const handleClick = (event) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };

  return (
    <div>
      <Button
        id="basic-button"
        aria-controls={open ? 'basic-menu' : undefined}
        aria-haspopup="true"
        aria-expanded={open ? 'true' : undefined}
        onClick={handleClick}
      >
        <NotificationsIcon
        sx={{color:'gray'}}
        fontSize='large'
        
        />
      </Button>
      <Menu
        id="basic-menu"
        anchorEl={anchorEl}
        open={open}
        onClose={handleClose}
        MenuListProps={{
          'aria-labelledby': 'basic-button',
        }}
      >
        <MenuItem sx={{fontSize:"22px" , fontWeight:"500" , ml:"100px" , mr:"100px"}} onClick={handleClose}>Notification</MenuItem>
        {notification.map((el,i)=>(
            <MenuItem onClick={handleClose} key={i}>{el}</MenuItem>
        ))}
      </Menu>
    </div>
  );
}
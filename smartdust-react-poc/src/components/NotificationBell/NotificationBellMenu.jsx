import * as React from 'react';
import Button from '@mui/material/Button';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import NotificationsIcon from '@mui/icons-material/Notifications';

export default function NotificationBellMenu() {
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
        {/* <MenuItem onClick={handleClose}>Notification</MenuItem> */}
        <MenuItem onClick={handleClose}>Sign in Successful!</MenuItem>
        <MenuItem onClick={handleClose}>UserName or password mismatch.</MenuItem>
        {/* <MenuItem onClick={handleClose}>UserName or password mismatch.</MenuItem> */}
      </Menu>
    </div>
  );
}
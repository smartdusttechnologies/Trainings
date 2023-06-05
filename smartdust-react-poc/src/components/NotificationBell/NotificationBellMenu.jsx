import * as React from 'react';
import './NotificationBellMenu.css'
import Button from '@mui/material/Button';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import NotificationsIcon from '@mui/icons-material/Notifications';
import AuthContext from '../../context/AuthProvider';
import { MenuList } from '@mui/material';

export default function NotificationBellMenu() {
  const {setNotification,notification} = React.useContext(AuthContext);
  const [readStatus, setReadStatus] = React.useState(Array(notification.length).fill(false));

    const [anchorEl, setAnchorEl] = React.useState(null);
    const open = Boolean(anchorEl);
  const handleClick = (event) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleReadStatus = (i)=>{
    const UpdatedReadStatus = [...readStatus]
    console.log(UpdatedReadStatus)
    console.log(i)
    UpdatedReadStatus[i] = !UpdatedReadStatus[i]
    setReadStatus(UpdatedReadStatus)
  }
  return (
    <div>
      <button
        id="basic-button"
        aria-controls={open ? 'basic-menu' : undefined}
        aria-haspopup="true"
        aria-expanded={open ? 'true' : undefined}
        onClick={handleClick}
        className='NotificationBell-button'
        current-count={notification.length}
      >
        <NotificationsIcon
        sx={{ width: 26, height: 26 ,color:'gray'}}        
        />
        {/* <div >{notification.length}</div> */}
      </button>
      <Menu
        id="basic-menu"
        anchorEl={anchorEl}
        open={open}
        onClose={handleClose}
        MenuListProps={{
          'aria-labelledby': 'basic-button',
        }}
        sx={{textAlign:"center",alignItems:"center"}}
      >
        <MenuItem sx={{color:"blue"}} onClick={()=>setNotification([]) }>clear</MenuItem>
        <MenuItem sx={{fontSize:"22px" , fontWeight:"500" ,minWidth:300,display:'block'}} onClick={handleClose}>Notification</MenuItem>
        {
          notification.length == 0 ?  <MenuItem sx={{display:'block'}}>No Notification</MenuItem> : <span></span>
        }
        {notification.map((el,i)=>(
          <MenuItem>
            <MenuItem 
            onClick={handleClose} 
            key={i} 
            style={{fontWeight: !readStatus[i] ? 'bold' : 500 , color: !readStatus[i] ? 'rgb(78, 78, 78)' : 'none'}}
            > {el} </MenuItem> 
            <Button onClick={()=>handleReadStatus(i)}>{readStatus[i] ? 'Unread' : 'Read'}</Button>
          </MenuItem>
        ))}
      </Menu>
    </div>
  );
}
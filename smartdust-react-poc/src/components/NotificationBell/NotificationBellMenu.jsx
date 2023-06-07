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
    setReadStatus(Array(notification.length).fill(true))
  };

  const handleReadStatus = (i)=>{
    const UpdatedReadStatus = [...readStatus]
    console.log(UpdatedReadStatus)
    console.log(i)
    UpdatedReadStatus[i] = !UpdatedReadStatus[i]
    setReadStatus(UpdatedReadStatus)
  };

  const handleViewAll = ()=>{
   setReadStatus(Array(notification.length).fill(true))
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
        <MenuItem sx={{fontSize:"22px" , fontWeight:"500" ,minWidth:300,display:'block'}} onClick={handleClose}>Notification</MenuItem>
        <MenuItem sx={{color:"blue",pt:0,pb:0}} onClick={()=>setNotification([]) }>clear</MenuItem>
        {
          notification.length == 0 ?  <MenuItem sx={{display:'block'}}>No Notification</MenuItem> : <span></span>
        }
        {notification.map((el,i)=>(
            <MenuItem 
              onClick={handleClose} 
              key={i} 
              style={{
                fontWeight: !readStatus[i] ? 'bold' : 500 ,
                backgroundColor: !readStatus[i] ? "rgb(253, 245, 245)":'white',
                color: el.success ? 'rgb(55, 169, 87)' : 'rgb(249, 64, 64)',
              }}
            > {el.message} </MenuItem> 
        ))}
        {
          notification.length > 0 ?  <MenuItem 
          sx={{
            fontSize:"18px" , fontWeight:"500" ,display:'block',bgcolor:"rgb(55, 169, 87)",color:"rgb(238, 238, 238)",
            '&:hover':{bgcolor:"rgb(55, 159, 87)"}
          }}
          onClick={handleViewAll}
          
         >View All</MenuItem> : <span></span>
        }
        
      </Menu>
    </div>
  );
}
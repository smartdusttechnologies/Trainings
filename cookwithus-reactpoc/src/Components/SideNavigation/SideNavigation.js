import React from 'react'
import { Divider, IconButton, List, Toolbar } from '@mui/material';
import MuiDrawer from '@mui/material/Drawer';
import { mainListItems, secondaryListItems } from './list';
import MenuIcon from '@mui/icons-material/Menu';
import ChevronLeftIcon from '@mui/icons-material/ChevronLeft';
import { styled, createTheme, ThemeProvider } from '@mui/material/styles';
import { useSelector, useDispatch } from "react-redux";
import { setisSideNavOpen } from '../../state';

const drawerWidth = 240;
const Drawer = styled(MuiDrawer, { shouldForwardProp: (prop) => prop !== 'open' })(
    ({ theme, open }) => ({
      '& .MuiDrawer-paper': {
        position: 'relative',
        whiteSpace: 'nowrap',
        width: drawerWidth,
        transition: theme.transitions.create('width', {
          easing: theme.transitions.easing.sharp,
          duration: theme.transitions.duration.enteringScreen,
        }),
        boxSizing: 'border-box',
        ...(!open && {
          overflowX: 'hidden',
          transition: theme.transitions.create('width', {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.leavingScreen,
          }),
          width: theme.spacing(7),
          [theme.breakpoints.up('sm')]: {
            width: theme.spacing(9),
          },
        }),
      },
    }),
  );

const SideNavigation = () => {
    const dispatch = useDispatch();
    const isSideNavOpen = useSelector((state) => state.cart.isSideNavOpen);

  return (
    <div>
        <Drawer variant="permanent" open={isSideNavOpen}>
          {/* <Toolbar
            sx={{
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'flex-end',
              px: [1],
            }}
          >
            <IconButton
                onClick={() => dispatch(setisSideNavOpen({}))} 
                sx={{
                  ...(!isSideNavOpen && { display: 'none' }),
                  }}
            >
              <ChevronLeftIcon />
            </IconButton>
          </Toolbar>
          <Divider /> */}
          <List component="nav">
            {mainListItems}
            <Divider sx={{ my: 1 }} />
            {secondaryListItems}
          </List>
        </Drawer>
    </div>
  )
}

export default SideNavigation

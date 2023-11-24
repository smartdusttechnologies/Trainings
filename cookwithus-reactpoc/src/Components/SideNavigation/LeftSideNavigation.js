import React from 'react'
import { CssBaseline, Divider, IconButton, List, Toolbar } from '@mui/material';
import MuiDrawer from '@mui/material/Drawer';
import { mainListItems, secondaryListItems } from './list';
import MenuIcon from '@mui/icons-material/Menu';
import ChevronLeftIcon from '@mui/icons-material/ChevronLeft';
import { styled, createTheme, ThemeProvider } from '@mui/material/styles';
import { useSelector, useDispatch } from "react-redux";
import { setisSideNavOpen } from '../../state';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import ListSubheader from '@mui/material/ListSubheader';
import Accordion from '@mui/material/Accordion';
import AccordionSummary from '@mui/material/AccordionSummary';
import AccordionDetails from '@mui/material/AccordionDetails';
import Typography from '@mui/material/Typography';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import DashboardIcon from '@mui/icons-material/Dashboard';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';
import PeopleIcon from '@mui/icons-material/People';
import BarChartIcon from '@mui/icons-material/BarChart';
import LayersIcon from '@mui/icons-material/Layers';
import HomeIcon from '@mui/icons-material/Home';
import { useNavigate } from 'react-router-dom';
import ExpandableAccordion from '../Accordion/Accordion';

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
  
  const lightTheme = createTheme({
    palette: {
      mode: 'light',
    },
  });
  
  const darkTheme = createTheme({
    palette: {
      mode: 'dark',
    },
  });

const LeftSideNavigation = () => {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const isSideNavOpen = useSelector((state) => state.cart.isSideNavOpen);
    const darkMode = useSelector((state) => state.cart.darkMode);

    const handleNavigationAndKeepMenuOpen = (route) => {
      navigate(route);
    };

  return (
      <ThemeProvider theme={darkMode ? darkTheme : lightTheme}>
        <CssBaseline/>
        <Drawer 
          variant="permanent" 
          open={isSideNavOpen}
          sx={{
            bgcolor: 'background.default',
            color: 'text.primary',
          }}
        >
          <List component="nav">
            <React.Fragment>
              <ListItemButton onClick={() => navigate('/')}>
                <ListItemIcon>
                  <HomeIcon />
                </ListItemIcon>
                <ListItemText primary="Home" />
              </ListItemButton>

              {/* ExpandableAccordion */}
              <ExpandableAccordion handleNavigationAndCloseMenu={handleNavigationAndKeepMenuOpen}/>
              
              <ListItemButton onClick={() => navigate('/cart')}>
                <ListItemIcon>
                  <ShoppingCartIcon />
                </ListItemIcon>
                <ListItemText primary="Cart" />
              </ListItemButton>
              <ListItemButton>
                <ListItemIcon>
                  <PeopleIcon />
                </ListItemIcon>
                <ListItemText primary="Customers" />
              </ListItemButton>
              <ListItemButton>
                <ListItemIcon>
                  <BarChartIcon />
                </ListItemIcon>
                <ListItemText primary="Reports" />
              </ListItemButton>
              <ListItemButton>
                <ListItemIcon>
                  <LayersIcon />
                </ListItemIcon>
                <ListItemText primary="Integrations" />
              </ListItemButton>
            </React.Fragment>
            <Divider sx={{ my: 1 }} />
            {secondaryListItems}
          </List>
        </Drawer>
      </ThemeProvider>  
  )
}

export default LeftSideNavigation

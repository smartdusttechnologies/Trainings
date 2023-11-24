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
import FacebookIcon from '@mui/icons-material/Facebook';
import EmailIcon from '@mui/icons-material/Email';
import LocalPhoneIcon from '@mui/icons-material/LocalPhone';
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

const RightSideNavigation = () => {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const isRightSideNavigationOpen = useSelector((state) => state.cart.isRightSideNavigationOpen);
    const darkMode = useSelector((state) => state.cart.darkMode);

    const handlePhoneClick = () => {
      window.location.href = `tel:7857068847`;
    };
  
    const handleEmailClick = () => {
      window.location.href = `mailto:your-email@example.com`;
    };
  
    const handleFavoritesClick = () => {
      window.location.href = 'https://www.facebook.com/people/Smartdust-Technologies/100071813210648/';
    };

  return (
      <ThemeProvider theme={darkMode ? darkTheme : lightTheme}>
        <CssBaseline/>
        <Drawer 
          variant="permanent" 
          open={isRightSideNavigationOpen}
          sx={{
            bgcolor: 'background.default',
            color: 'text.primary',
          }}
        >
          <List component="nav">
            <React.Fragment>
              <ListItemButton onClick={() => handlePhoneClick()}>
                <ListItemIcon>
                  <LocalPhoneIcon sx={{color:'rgb(39, 197, 60)'}} />
                </ListItemIcon>
                <ListItemText primary="Phone" />
              </ListItemButton>
              <ListItemButton onClick={() => handleEmailClick()}>
                <ListItemIcon>
                  <EmailIcon sx={{color:'rgb(161, 34, 34)'}} />
                </ListItemIcon>
                <ListItemText primary="Email" />
              </ListItemButton>
              <ListItemButton onClick={() => handleFavoritesClick()}>
                <ListItemIcon>
                  <FacebookIcon sx={{color:'rgb(11, 83, 207)'}} />
                </ListItemIcon>
                <ListItemText primary="Facebook" />
              </ListItemButton>
            </React.Fragment>
            <Divider sx={{ my: 1 }} />
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
          </List>
        </Drawer>
      </ThemeProvider>  
  )
}

export default RightSideNavigation

import React from 'react'
import { Box, Button, CssBaseline, Divider, IconButton, ThemeProvider, Typography, createTheme } from "@mui/material";
import { useSelector, useDispatch } from "react-redux";
import CloseIcon from "@mui/icons-material/Close";
import styled from "@emotion/styled";
import { useNavigate } from "react-router-dom";
import { setisMenuOpen } from "../../state";
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';
import PeopleIcon from '@mui/icons-material/People';
import BarChartIcon from '@mui/icons-material/BarChart';
import LayersIcon from '@mui/icons-material/Layers';
import HomeIcon from '@mui/icons-material/Home';
import ExpandableAccordion from '../Accordion/Accordion';
import Accordion from '@mui/material/Accordion';
import AccordionSummary from '@mui/material/AccordionSummary';
import AccordionDetails from '@mui/material/AccordionDetails';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import RestaurantIcon from '@mui/icons-material/Restaurant';
import FastfoodIcon from '@mui/icons-material/Fastfood';
import IcecreamIcon from '@mui/icons-material/Icecream';

const FlexBox = styled(Box)`
  display: flex;
  justify-content: space-between;
  align-items: center;
`;
  
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

const ShowMoreMenu = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const isMenuOpen = useSelector((state) => state.cart.isMenuOpen);
  const darkMode = useSelector((state) => state.cart.darkMode);

  const handleNavigationAndCloseMenu = (route) => {
    navigate(route);
    dispatch(setisMenuOpen({}));
  };

  return (
      <ThemeProvider theme={darkMode ? darkTheme : lightTheme}>
        <CssBaseline/>
        <Box
          display={isMenuOpen ? "block" : "none"}
          position="fixed"
          zIndex={10}
          width="100%"
          height="100%"
          left="0"
          top="0"
          overflow="auto"
          sx={{
          '@media (min-width: 500px)': {
            display:'none'
          },
          bgcolor: 'background.default',
          color: 'text.primary',
          }}
        >
          <Box
            position="fixed"
            left="0"
            bottom="0"
            width="100%"
            height="100%"
            sx={{
              bgcolor: 'background.default',
              color: 'text.primary',
            }}
          >
            <Box padding="30px" overflow="auto" height="100%">
              {/* HEADER */}
              <FlexBox mb="15px">
                <Typography variant="h5"> </Typography>
                <IconButton onClick={() => dispatch(setisMenuOpen({}))}>
                  <CloseIcon />
                </IconButton>
              </FlexBox>

              {/* Actions  */}
              <Box width='80%' margin='auto'>
                <React.Fragment>
                    <ListItemButton onClick={() => handleNavigationAndCloseMenu('/')}>
                      <ListItemIcon>
                        <HomeIcon />
                      </ListItemIcon>
                      <ListItemText primary="Home" />
                    </ListItemButton>

                    {/* ExpandableAccordion */}
                    <Accordion sx={{boxShadow:'none'}}>
                        <AccordionSummary
                            expandIcon={<ExpandMoreIcon />}
                            aria-controls="panel1a-content"
                        >
                            <ListItemButton sx={{ padding: 0, '&:hover': { backgroundColor: 'transparent' } }}>
                                <ListItemIcon>
                                    <RestaurantIcon />
                                </ListItemIcon>
                            <ListItemText primary="Products" />
                            </ListItemButton>
                        </AccordionSummary>
                        <AccordionDetails>
                                <ListItemButton onClick={() => handleNavigationAndCloseMenu('/meals')}>
                                    <ListItemIcon>
                                        <FastfoodIcon />
                                    </ListItemIcon>
                                    <ListItemText primary="Fast Food" />
                                </ListItemButton>
                                <Divider/>
                                <ListItemButton onClick={() => handleNavigationAndCloseMenu('/meals')}>
                                  <ListItemIcon>
                                      <IcecreamIcon />
                                  </ListItemIcon>
                                  <ListItemText primary="Icecream" />
                                </ListItemButton>
                        </AccordionDetails>
                    </Accordion>
                    
                    <ListItemButton onClick={() => handleNavigationAndCloseMenu('/cart')}>
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
                </Box>

            </Box>
          </Box>
        </Box>
      </ThemeProvider>
  );
};

export default ShowMoreMenu;

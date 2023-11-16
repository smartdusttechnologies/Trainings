import React from 'react'
import { Box, Button, CssBaseline, Divider, IconButton, ThemeProvider, Typography, createTheme } from "@mui/material";
import { useSelector, useDispatch } from "react-redux";
import CloseIcon from "@mui/icons-material/Close";
import styled from "@emotion/styled";
import { useNavigate } from "react-router-dom";
import { setisBottomNavMenuOpen } from "../../state";
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

const BottomNavThreedot = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const isBottomNavMenuOpen = useSelector((state) => state.cart.isBottomNavMenuOpen);
  const darkMode = useSelector((state) => state.cart.darkMode);

  return (
      <ThemeProvider theme={darkMode ? darkTheme : lightTheme}>
        <CssBaseline/>
        <Box
          display={isBottomNavMenuOpen ? "block" : "none"}
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
                <IconButton onClick={() => dispatch(setisBottomNavMenuOpen({}))}>
                  <CloseIcon />
                </IconButton>
              </FlexBox>

            </Box>
          </Box>
        </Box>
      </ThemeProvider>
  );
};

export default BottomNavThreedot;

import React from 'react'
import { Box, Button, CssBaseline, Divider, IconButton, ThemeProvider, Typography, createTheme } from "@mui/material";
import { useSelector, useDispatch } from "react-redux";
import CloseIcon from "@mui/icons-material/Close";
import styled from "@emotion/styled";
import { useNavigate } from "react-router-dom";
import { setisBottomNavMenuOpen } from "../../state";
import { secondaryListItems } from '../SideNavigation/list';

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

const ThreeDotBottomNav = () => {
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

              {/* Actions  */}
              <Box width='80%' margin='auto'>
                {secondaryListItems}
              </Box>

            </Box>
          </Box>
        </Box>
      </ThemeProvider>
  );
};

export default ThreeDotBottomNav;

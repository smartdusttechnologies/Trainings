import './App.css';
import BottomNav from './Components/BottomNavigation/BottomNav';
import Footer from './Components/Footer/Footer';
import ShowMoreMenu from './Components/Menu/ShowMoreMenu';
import NavBar from './Components/NavBar/NavBar';
import AllRoutes from './Components/Routes/AllRoutes';
import LeftSideNavigation from './Components/SideNavigation/LeftSideNavigation';
import Box from '@mui/material/Box';
import ThreeDotBottomNav from './Components/BottomNavigation/ThreeDotBottomNav';
import RightSideNavigation from './Components/SideNavigation/RightSideNavigation';

function App() {
  return (
      <div>
        <NavBar/>
        <Box sx={{ display: 'flex' }}>
          <Box
           sx={{
            '@media (max-width: 500px)': {
              display:'none'
            },
           }}
          >
            <LeftSideNavigation/>
          </Box>
          <AllRoutes/>
          <Box
           sx={{
            '@media (max-width: 500px)': {
              display:'none'
            },
           }}
          >
            <RightSideNavigation/>
          </Box>
        </Box>
        <ShowMoreMenu/>
        <ThreeDotBottomNav/>
        <Footer/>
        <BottomNav/>
      </div>
  );
}

export default App;

import './App.css';
import BottomNav from './Components/BottomNavigation/BottomNav';
import Footer from './Components/Footer/Footer';
import ShowMoreMenu from './Components/Menu/ShowMoreMenu';
import NavBar from './Components/NavBar/NavBar';
import AllRoutes from './Components/Routes/AllRoutes';
import SideNavigation from './Components/SideNavigation/SideNavigation';
import Box from '@mui/material/Box';
import BottomNavThreedot from './Components/ThreedotDrawer/BottomNavThreedot';
import DesktopBottomNav from './Components/BottomNavigation/DesktopBottomNav';

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
            <SideNavigation/>
          </Box>
          <AllRoutes/>
          <Box
           sx={{
            '@media (max-width: 500px)': {
              display:'none'
            },
           }}
          >
            <DesktopBottomNav/>
          </Box>
        </Box>
        <ShowMoreMenu/>
        <BottomNavThreedot/>
        <Footer/>
        <BottomNav/>
      </div>
  );
}

export default App;

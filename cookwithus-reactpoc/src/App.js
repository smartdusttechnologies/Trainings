import './App.css';
import BottomNav from './Components/BottomNavigation/BottomNav';
import Footer from './Components/Footer/Footer';
import NavBar from './Components/NavBar/NavBar';
import AllRoutes from './Components/Routes/AllRoutes';
import SideNavigation from './Components/SideNavigation/SideNavigation';
import Box from '@mui/material/Box';

function App() {
  return (
      <div>
        <NavBar/>
        <Box sx={{ display: 'flex' }}>
          <SideNavigation/>
          <AllRoutes/>
        </Box>
        <Footer/>
        <BottomNav/>
      </div>
  );
}

export default App;

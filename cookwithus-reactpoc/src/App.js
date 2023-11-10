import './App.css';
import Footer from './Components/Footer/Footer';
import NavBar from './Components/NavBar/NavBar';
import AllRoutes from './Components/Routes/AllRoutes';
import SideNavigation from './Components/SideNavigation/SideNavigation';
import { ThemeProvider } from './context/ThemeContext';

function App() {
  return (
    <ThemeProvider>
      <div>
        <NavBar/>
        <SideNavigation/>
        <AllRoutes/>
        <Footer/>
      </div>
    </ThemeProvider>
  );
}

export default App;

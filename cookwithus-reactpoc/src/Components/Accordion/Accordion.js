import React from 'react'
import Accordion from '@mui/material/Accordion';
import AccordionSummary from '@mui/material/AccordionSummary';
import AccordionDetails from '@mui/material/AccordionDetails';
import Typography from '@mui/material/Typography';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import DashboardIcon from '@mui/icons-material/Dashboard';
import HomeIcon from '@mui/icons-material/Home';
import { Divider } from '@mui/material';
import RestaurantIcon from '@mui/icons-material/Restaurant';
import FastfoodIcon from '@mui/icons-material/Fastfood';
import IcecreamIcon from '@mui/icons-material/Icecream';

const ExpandableAccordion = ({handleNavigationAndCloseMenu}) => {
    
  return (
    <div>
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
    </div>
  )
}

export default ExpandableAccordion

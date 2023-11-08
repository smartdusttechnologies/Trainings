import React, { useState } from 'react';
import LocationOnRoundedIcon from '@mui/icons-material/LocationOnRounded';
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, Divider, IconButton, Tooltip } from '@mui/material';

function LocationSelector() {
  const [openDialog, setOpenDialog] = useState(false);
  const [selectedLocation, setSelectedLocation] = useState('');

  const handleOpenDialog = () => {
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
  };

  const handleLocationSelection = () => {
        navigator.geolocation.getCurrentPosition((position) => {
          const userLocation = {
            latitude: position.coords.latitude,
            longitude: position.coords.longitude,
          };
      
          console.log('User location:', userLocation);
        });
  };

  return (
    <div>
      <Tooltip title="Select Location">
        <IconButton size="large" color="inherit" onClick={handleOpenDialog}>
          <LocationOnRoundedIcon sx={{ width: 26, height: 26,color:'white' }} />
        </IconButton>
      </Tooltip>

      <Dialog open={openDialog} onClose={handleCloseDialog}>
        <DialogTitle>Select Your Location</DialogTitle>
        <Divider/>
        <DialogContent>
            <Button 
            variant="outlined"
            size="small"
            onClick={handleLocationSelection}
            >
                Click here to pick your current location
            </Button>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleLocationSelection} color="primary">
            Save
          </Button>
          <Button onClick={handleCloseDialog} color="error">
            Cancel
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
}

export default LocationSelector;

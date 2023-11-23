import React, { useState } from 'react';
import LocationOnRoundedIcon from '@mui/icons-material/LocationOnRounded';
import { Box, Button, Dialog, DialogActions, DialogContent, DialogTitle, Divider, IconButton, Tooltip, Typography } from '@mui/material';
import axios from 'axios';

function LocationSelector() {
  const [openDialog, setOpenDialog] = useState(false);
  const [latitude , setLatitude] = useState(0);
  const [longitude , setLongitude] = useState(0);
  const [selectedLocation, setSelectedLocation] = useState({
    latitude: 0,
    longitude: 0,
    address: '',
  });

  const handleOpenDialog = () => {
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
  };

  const handleLocationSelection = () => {
        navigator.geolocation.getCurrentPosition((position) => {
          setLatitude(position.coords.latitude)
          setLongitude(position.coords.longitude)
          console.log(position.coords.latitude , position.coords.longitude , 'position.coords.')
        });

        const apiKey = 'c5b7441c5f00403e91fadb5fa6db09ef';
        const geocodingUrl = `https://api.opencagedata.com/geocode/v1/json?q=${latitude}+${longitude}&key=${apiKey}`;

        axios.get(geocodingUrl)
          .then((response) => {
            console.log(response)
            const results = response.data.results;
            if (results.length > 0) {
              console.log(results[0].formatted)
              const address = results[0].formatted;
              setSelectedLocation({
                latitude,
                longitude,
                address,
              });
            } else {
              console.error('No results found');
            }
            // setLoading(false);
          })
          .catch((error) => {
            console.error('Error fetching address:', error);
            // setLoading(false);
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

            {selectedLocation.address && (
              <Box>
                <Typography>
                  Address: {selectedLocation.address}
                </Typography>
              </Box>
            )}
            
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

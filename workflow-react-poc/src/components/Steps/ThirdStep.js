import React from "react";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import { FormControl, Grid, InputLabel, MenuItem, Select } from "@mui/material";

const ThirdStep = () => {
  return (
    <div>
      <Typography variant="h6" gutterBottom>
        Payment details
      </Typography>
      <Grid container spacing={3}>
        <Grid item xs={12}>
          <FormControl fullWidth>
            <InputLabel id="demo-select-small-label">Select Type</InputLabel>
            <Select required id="method" name="method" label="Select Type">
              <MenuItem value={"credit"}>Credit Card</MenuItem>
              <MenuItem value={"debit"}>Debit Card</MenuItem>
              <MenuItem value={"upi"}>UPI</MenuItem>
            </Select>
          </FormControl>
        </Grid>
        <Grid item xs={12}>
          <FormControl fullWidth>
            <InputLabel id="demo-select-small-label">Select UI</InputLabel>
            <Select required id="method" name="method" label="Select Type">
              <MenuItem value={"credit"}>Credit Card</MenuItem>
              <MenuItem value={"debit"}>Debit Card</MenuItem>
              <MenuItem value={"upi"}>UPI</MenuItem>
            </Select>
          </FormControl>
        </Grid>
        <Grid item xs={12}>
          <TextField
            required
            id="type"
            name="type"
            label="Type"
            fullWidth
            autoComplete="type"
          />
        </Grid>
      </Grid>
    </div>
  );
};

export default ThirdStep;
